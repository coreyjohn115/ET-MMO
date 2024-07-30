namespace ET.Server;

[Event(SceneType.Map)]
public class UnitDoAttack_ProcessBuff: AEvent<Scene, UnitDoAttack>
{
    protected override async ETTask Run(Scene scene, UnitDoAttack a)
    {
        a.Unit.GetComponent<BuffComponent>().ProcessBuff(BuffEvent.PerHurt, a.HurtList, a.IsPhysics, a.Element);
        await ETTask.CompletedTask;
    }
}