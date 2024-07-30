namespace ET.Server;

[Event(SceneType.Map)]
public class UnitEnterGame_UpdateSkill: AEvent<Scene, UnitEnterGame>
{
    protected override async ETTask Run(Scene scene, UnitEnterGame a)
    {
        a.Unit.GetComponent<SkillComponent>().UpdateSkillList();
        await ETTask.CompletedTask;
    }
}