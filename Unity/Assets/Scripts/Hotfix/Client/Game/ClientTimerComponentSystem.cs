namespace ET.Client
{
    [EntitySystemOf(typeof (ClientTimerComponent))]
    [FriendOf(typeof (ClientTimerComponent))]
    public static partial class ClientTimerComponentSystem
    {
        [Invoke(TimerInvokeType.Client0_1S)]
        private class ClientHeart0_1Timer: ATimer<ClientTimerComponent>
        {
            protected override void Run(ClientTimerComponent t)
            {
                EventSystem.Instance.Publish(t.Root(), new ClientHeart0_1());
            }
        }

        [Invoke(TimerInvokeType.Client0_5S)]
        private class ClientHeart0_5Timer: ATimer<ClientTimerComponent>
        {
            protected override void Run(ClientTimerComponent t)
            {
                EventSystem.Instance.Publish(t.Root(), new ClientHeart0_5());
            }
        }

        [Invoke(TimerInvokeType.Client1S)]
        private class ClientHeart1Timer: ATimer<ClientTimerComponent>
        {
            protected override void Run(ClientTimerComponent t)
            {
                EventSystem.Instance.Publish(t.Root(), new ClientHeart1());
            }
        }

        [Invoke(TimerInvokeType.Client5S)]
        private class ClientHeart5Timer: ATimer<ClientTimerComponent>
        {
            protected override void Run(ClientTimerComponent t)
            {
                EventSystem.Instance.Publish(t.Root(), new ClientHeart5());
            }
        }

        [EntitySystem]
        private static void Awake(this ClientTimerComponent self)
        {
            TimerComponent time = self.Root().GetComponent<TimerComponent>();
            self.timer0_1 = time.NewRepeatedTimer(100, TimerInvokeType.Client0_1S, self);
            self.timer0_5 = time.NewRepeatedTimer(500, TimerInvokeType.Client0_5S, self);
            self.timer1 = time.NewRepeatedTimer(1000, TimerInvokeType.Client1S, self);
            self.timer5 = time.NewRepeatedTimer(5000, TimerInvokeType.Client5S, self);
        }

        [EntitySystem]
        private static void Destroy(this ClientTimerComponent self)
        {
            TimerComponent time = self.Root().GetComponent<TimerComponent>();
            time.Remove(ref self.timer0_1);
            time.Remove(ref self.timer0_5);
            time.Remove(ref self.timer1);
            time.Remove(ref self.timer5);
        }
    }
}