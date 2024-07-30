namespace ET.Client
{
    [Event(SceneType.Client)]
    public class AppStartInitFinish_CreateTools: AEvent<Scene, AppStartInitFinish>
    {
        protected override async ETTask Run(Scene root, AppStartInitFinish args)
        {
            root.GetComponent<UIComponent>().ShowWindowAsync(WindowID.Win_UITools).NoContext();
            await ETTask.CompletedTask;
        }
    }
}