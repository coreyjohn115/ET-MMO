using System;
using System.Net;
using System.Net.Sockets;

namespace ET.Client
{
    [MessageHandler(SceneType.NetClient)]
    public class Main2NetClient_LoginHandler: MessageHandler<Scene, Main2NetClient_Login, NetClient2Main_Login>
    {
        protected override async ETTask Run(Scene root, Main2NetClient_Login request, NetClient2Main_Login response)
        {
            string account = request.Account;
            string password = request.Password;
            root.RemoveComponent<RouterAddressComponent>();
            RouterAddressComponent routerAddressComponent =
                    root.AddComponent<RouterAddressComponent, string, int>(request.RouterHttpHost, ConstValue.RouterHttpPort);
            await routerAddressComponent.Init();
            NetworkProtocol protocol = NetworkProtocol.UDP;
#if UNITY_WEBGL
            protocol = NetworkProtocol.Websocket;
#endif
            root.AddComponent<NetComponent, AddressFamily, NetworkProtocol>(routerAddressComponent.RouterManagerIPAddress.AddressFamily,
                protocol);
            root.GetComponent<FiberParentComponent>().ParentFiberId = request.OwnerFiberId;

            NetComponent netComponent = root.GetComponent<NetComponent>();
            IPEndPoint realmAddress = routerAddressComponent.GetRealmAddress(account);

            R2C_Login r2CLogin;
            using (Session session = await netComponent.CreateRouterSession(realmAddress, account, password))
            {
                C2R_Login c2RLogin = C2R_Login.Create();
                c2RLogin.Zone = request.Zone;
                c2RLogin.Account = account;
                c2RLogin.Password = password;
                r2CLogin = (R2C_Login)await session.Call(c2RLogin);
            }

            if (r2CLogin.Error != ErrorCode.ERR_Success)
            {
                response.Error = r2CLogin.Error;
                response.Message = r2CLogin.Message;
                return;
            }

            // 创建一个gate Session,并且保存到SessionComponent中
            Session gateSession = await netComponent.CreateRouterSession(NetworkHelper.ToIPEndPoint(r2CLogin.Address), account, password);
            gateSession.AddComponent<ClientSessionErrorComponent>();
            root.AddComponent<SessionComponent>().Session = gateSession;
            C2G_LoginGate loginGate = C2G_LoginGate.Create();
            loginGate.Key = r2CLogin.Key;
            loginGate.GateId = r2CLogin.GateId;
            loginGate.Id = request.Id;
            G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await gateSession.Call(loginGate);

            if (g2CLoginGate.Error != ErrorCode.ERR_Success)
            {
                response.Error = g2CLoginGate.Error;
                response.Message = g2CLoginGate.Message;
                return;
            }

            response.PlayerId = g2CLoginGate.PlayerId;
            Log.Debug("登陆gate成功!");
        }
    }
}