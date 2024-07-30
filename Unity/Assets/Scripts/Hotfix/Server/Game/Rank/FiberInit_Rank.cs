namespace ET.Server
{
    [Invoke((long)SceneType.Rank)]
    public class FiberInit_Rank: AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            root.AddComponent<ProcessInnerSender>();
            root.AddComponent<MessageLocationSenderComponent>();
            root.AddComponent<MessageSender>();
            root.AddComponent<LocationProxyComponent>();
            root.AddComponent<DBManagerComponent>();

            root.AddComponent<RankComponent>();
            await ETTask.CompletedTask;
        }
    }
}