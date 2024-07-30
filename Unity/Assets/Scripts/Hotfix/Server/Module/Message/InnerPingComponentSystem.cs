namespace ET.Server
{
    [EntitySystemOf(typeof (InnerPingComponent))]
    [FriendOf(typeof (InnerPingComponent))]
    public static partial class InnerPingComponentSystem
    {
        [Invoke(TimerInvokeType.InnerPing)]
        public class InnerPingComponentTimer: ATimer<InnerPingComponent>
        {
            protected override void Run(InnerPingComponent self)
            {
                self.Check().NoContext();
            }
        }

        public static async ETTask Check(this InnerPingComponent self)
        {
            string[] localIP = NetworkHelper.GetAddressIPs();
            var processConfigs = StartProcessConfigCategory.Instance.GetAll();
            foreach (StartProcessConfig startProcessConfig in processConfigs.Values)
            {
                if (!WatcherHelper.IsThisMachine(startProcessConfig.InnerIP, localIP))
                {
                    continue;
                }

                if (self.Fiber().Process == startProcessConfig.Id)
                {
                    continue;
                }

                var resp = await self.Scene().GetComponent<ProcessOuterSender>()
                        .Call(new ActorId(startProcessConfig.Id, ConstFiberId.NetInner), InnerPingRequest.Create());
            }
        }

        [EntitySystem]
        private static void Awake(this InnerPingComponent self)
        {
            self.Timer = self.Fiber().Root.GetComponent<TimerComponent>().NewRepeatedTimer(3000, TimerInvokeType.InnerPing, self);
        }

        [EntitySystem]
        private static void Destroy(this InnerPingComponent self)
        {
            self.Fiber().Root.GetComponent<TimerComponent>()?.Remove(ref self.Timer);
        }
    }
}