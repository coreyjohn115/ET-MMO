namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class M2C_StartSceneChangeHandler: MessageHandler<Scene, M2C_StartSceneChange>
    {
        protected override async ETTask Run(Scene root, M2C_StartSceneChange message)
        {
            root.RemoveComponent<AIComponent>();
            CurrentScenesComponent currentScenesComponent = root.GetComponent<CurrentScenesComponent>();
            bool isFirst = currentScenesComponent.Scene == null;
            currentScenesComponent.Scene?.Dispose(); // 删除之前的CurrentScene，创建新的
            Scene currentScene = CurrentSceneFactory.Create(message.SceneInstanceId, message.SceneName, currentScenesComponent);
            UnitComponent unitComponent = currentScene.AddComponent<UnitComponent>();

            // 可以订阅这个事件中创建Loading界面
            EventSystem.Instance.Publish(root, new SceneChangeStart());

            // 等待CreateMyUnit的消息
            Wait_CreateMyUnit waitCreateMyUnit = await root.GetComponent<ObjectWait>().Wait<Wait_CreateMyUnit>();
            M2C_CreateMyUnit m2CCreateMyUnit = waitCreateMyUnit.Message;
            Unit unit = UnitFactory.Create(currentScene, m2CCreateMyUnit.Unit, true);
            unitComponent.Add(unit);

            await root.GetComponent<TimerComponent>().WaitAsync(500);
            EventSystem.Instance.Publish(root, new LoadingProgress() { Progress = 1 });
            await EventSystem.Instance.PublishAsync(root, new SceneChangeFinish() { IsFirst = isFirst });
            // 通知等待场景切换的协程
            root.GetComponent<ObjectWait>().Notify(new Wait_SceneChangeFinish());
        }
    }
}