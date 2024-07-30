namespace ET.Client
{
    [Event(SceneType.Client)]
    public class RemoveItemEvent_Watcher: AEvent<Scene, RemoveItemEvent>
    {
        protected override async ETTask Run(Scene scene, RemoveItemEvent a)
        {
            ItemWatcherComponent.Instance.Run(a.Item, a);
            await ETTask.CompletedTask;
        }
    }
}