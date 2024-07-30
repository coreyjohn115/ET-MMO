namespace ET.Server
{
    [Invoke((long)SceneType.Activity)]
    public class FiberInit_Activity: AInvokeHandler<FiberInit, ETTask>
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

            root.AddComponent<ActivityComponent>();
            await ETTask.CompletedTask;
        }
    }
}