namespace ET.Server;

/// <summary>
/// 基础信息改变事件
/// </summary>
[Event(SceneType.Map)]
public class BasicChangeEvent_Update: AEvent<Scene, BasicChangeEvent>
{
    protected override async ETTask Run(Scene scene, BasicChangeEvent a)
    {
        a.Unit.SyncBasicInfo();
        await ETTask.CompletedTask;
    }
}