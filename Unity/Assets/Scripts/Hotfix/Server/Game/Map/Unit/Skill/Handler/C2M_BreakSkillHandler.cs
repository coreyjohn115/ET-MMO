namespace ET.Server;

/// <summary>
/// 使用技能
/// </summary>
[MessageLocationHandler(SceneType.Map)]
public class C2M_BreakSkillHandler: MessageLocationHandler<Unit, C2M_BreakSkill>
{
    protected override async ETTask Run(Unit unit, C2M_BreakSkill request)
    {
        await ETTask.CompletedTask;
        unit.GetComponent<SkillComponent>().BreakSkill(true);
    }
}