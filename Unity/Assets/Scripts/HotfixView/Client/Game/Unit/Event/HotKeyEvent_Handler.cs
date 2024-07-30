namespace ET.Client
{
    [Event(SceneType.Current)]
    public class HotKeyEvent_Handler: AEvent<Scene, HotKeyEvent>
    {
        protected override async ETTask Run(Scene scene, HotKeyEvent e)
        {
            await ETTask.CompletedTask;
            Log.Info(e.KeyCode);
        }
    }
}