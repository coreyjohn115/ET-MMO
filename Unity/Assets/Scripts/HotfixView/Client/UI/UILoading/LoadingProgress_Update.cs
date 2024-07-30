namespace ET.Client
{
    [Event(SceneType.Client)]
    public class LoadingProgress_Update: AEvent<Scene, LoadingProgress>
    {
        protected override async ETTask Run(Scene scene, LoadingProgress a)
        {
            scene.GetComponent<UIComponent>().GetDlgLogic<UILoading>().UpdateProcess(a.Progress);
            await ETTask.CompletedTask;
        }
    }
}