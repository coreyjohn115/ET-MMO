using System;
using System.Net;

namespace ET.Client
{
    [EntitySystemOf(typeof (RouterCheckComponent))]
    public static partial class RouterCheckComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RouterCheckComponent self)
        {
        }

        [Event(SceneType.Client)]
        private class RouterCheckEvent: AEvent<Scene, ClientHeart1>
        {
            protected override async ETTask Run(Scene scene, ClientHeart1 a)
            {
                if (!scene.GetComponent<SessionComponent>())
                {
                    return;
                }

                Session session = scene.GetComponent<SessionComponent>().Session;
                await session?.GetComponent<RouterCheckComponent>().Check();
            }
        }

        private static async ETTask Check(this RouterCheckComponent self)
        {
            Session session = self.GetParent<Session>();
            Scene root = self.Root();
            IPEndPoint realAddress = session.RemoteAddress;
            NetComponent netComponent = root.GetComponent<NetComponent>();
            long time = TimeInfo.Instance.Frame;
            if (time - session.LastRecvTime < 7 * 1000)
            {
                return;
            }

            try
            {
                long sessionId = session.Id;
                (uint localConn, uint remoteConn) = session.AService.GetChannelConn(sessionId);
                Log.Info($"get recvLocalConn start: {root.Id} {realAddress} {localConn} {remoteConn}");
                (uint recvLocalConn, IPEndPoint routerAddress) = await netComponent.GetRouterAddress(realAddress, localConn, remoteConn);
                if (recvLocalConn == 0)
                {
                    Log.Error($"get recvLocalConn fail: {root.Id} {routerAddress} {realAddress} {localConn} {remoteConn}");
                    return;
                }

                Log.Info($"get recvLocalConn ok: {root.Id} {routerAddress} {realAddress} {recvLocalConn} {localConn} {remoteConn}");
                session.LastRecvTime = TimeInfo.Instance.Now();
                session.AService.ChangeAddress(sessionId, routerAddress);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}