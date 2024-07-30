namespace ET.Server;

[Event(SceneType.Map)]
public class UnitEnterGameOk_Event: AEvent<Scene, UnitEnterGameOk>
{
    protected override async ETTask Run(Scene scene, UnitEnterGameOk a)
    {
        a.Unit.ResetHp();
        await ETTask.CompletedTask;
    }
}