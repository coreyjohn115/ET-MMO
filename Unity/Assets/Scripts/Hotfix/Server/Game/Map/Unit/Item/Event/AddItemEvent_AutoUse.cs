namespace ET.Server;

[Event(SceneType.Map)]
public class AddItemEvent_AutoUse: AEvent<Scene, AddItemEvent>
{
    protected override async ETTask Run(Scene scene, AddItemEvent a)
    {
        await ETTask.CompletedTask;
    }
}