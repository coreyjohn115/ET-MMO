namespace ET.Client
{
    [Event(SceneType.Client)]
    public class AddItemEvent_Watcher: AEvent<Scene, AddItemEvent>
    {
        protected override async ETTask Run(Scene scene, AddItemEvent a)
        {
            ItemWatcherComponent.Instance.Run(a.Item, a);
            await ETTask.CompletedTask;
        }
    }
}