namespace ET.Client
{
    [Event(SceneType.Client)]
    public class SceneChangeFinish_CloseLoading: AEvent<Scene, SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish a)
        {
            scene.GetComponent<UIComponent>().CloseWindow(WindowID.Win_UILoading);
            await ETTask.CompletedTask;
        }
    }
}