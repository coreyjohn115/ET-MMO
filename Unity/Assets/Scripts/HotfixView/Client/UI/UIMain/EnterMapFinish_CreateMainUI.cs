namespace ET.Client
{
    [Event(SceneType.Client)]
    public class EnterMapFinish_CreateMainUI: AEvent<Scene, EnterMapFinish>
    {
        protected override async ETTask Run(Scene scene, EnterMapFinish e)
        {
            await scene.GetComponent<UIComponent>().ShowWindowAsync(WindowID.Win_UIMain);
            scene.GetComponent<UIComponent>().CloseWindow(WindowID.Win_UILogin);
        }
    }
}