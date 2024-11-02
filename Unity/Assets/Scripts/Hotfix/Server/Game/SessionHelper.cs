namespace ET.Server
{
    public static class SessionHelper
    {
        public static async ETTask Kick(this Session session, int kickType = 1)
        {
            session.GetComponent<SessionPlayerComponent>().Kick = true;
            G2C_Kick kick = G2C_Kick.Create();
            kick.KickType = kickType;
            session.Send(kick);
            await session.Disconnect();
        }

        public static async ETTask Disconnect(this Session self)
        {
            if (self == null || self.IsDisposed)
            {
                return;
            }

            var lastId = self.InstanceId;
            await self.Root().GetComponent<TimerComponent>().WaitAsync(1000);
            if (lastId != self.InstanceId)
            {
                return;
            }

            self.Dispose();
        }
    }
}