namespace ET.Client
{
    [Invoke((long)SceneType.Robot)]
    public class FiberInit_Robot: AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            root.AddComponent<ProcessInnerSender>();
            root.AddComponent<ClientPlayerComponent>();
            root.AddComponent<CurrentScenesComponent>();
            root.AddComponent<ObjectWait>();

            string url = StartSceneConfigCategory.Instance.Account.InnerIp;
            int error = await LoginHelper.GetAppSetting(root, true, url);
            if (error != ErrorCode.ERR_Success)
            {
                Log.Error($"GetAppSetting error {error}!");
                await FiberManager.Instance.Remove(fiberInit.Fiber.Id);
                return;
            }

            root.SceneType = SceneType.Client;
            await EventSystem.Instance.PublishAsync(root, new AppStartInitFinish() { IsRobot = true });
            root.AddComponent<AIComponent, int>(1);
        }
    }
}