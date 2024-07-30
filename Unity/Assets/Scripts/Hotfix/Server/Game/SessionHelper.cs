namespace ET.Server
{
    public static class SessionHelper
    {
        public static async ETTask Disconnect(this Session self)
        {
            if (self == null || self.IsDisposed)
            {
                return;
            }

            var lastId = self.InstanceId;
            await self.Fiber().Root.GetComponent<TimerComponent>().WaitAsync(1000);
            if (lastId != self.InstanceId)
            {
                return;
            }

            self.Dispose();
        }
    }
}