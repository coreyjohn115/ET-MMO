namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    public class C2R_LoginHandler: MessageSessionHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response)
        {
            // 随机分配一个Gate
            StartSceneConfig config = RealmGateAddressHelper.GetGate(request.Zone, request.Account);
            Log.Debug($"gate address: {config}");

            // 向gate请求一个key,客户端可以拿着这个key连接gate
            R2G_GetLoginKey proto = R2G_GetLoginKey.Create();
            proto.Account = request.Account;
            var g2RGetLoginKey = await session.Scene().GetComponent<MessageSender>().Call<G2R_GetLoginKey>(config.ActorId, proto);

            response.Address = config.InnerIPPort.ToString();
            response.Key = g2RGetLoginKey.Key;
            response.GateId = g2RGetLoginKey.GateId;
            session.Disconnect().NoContext();
        }
    }
}