namespace ET.Client
{
    [Event(SceneType.Client)]
    public class AppStartInitFinish_Menu: AEvent<Scene, AppStartInitFinish>
    {
        protected override async ETTask Run(Scene root, AppStartInitFinish args)
        {
            root.AddComponent<MenuComponent>();
            await ETTask.CompletedTask;
        }
    }
}