namespace ET.Server;

/// <summary>
/// 使用技能
/// </summary>
[MessageLocationHandler(SceneType.Map)]
public class C2M_UseSkillHandler: MessageLocationHandler<Unit, C2M_UseSkillRequest, M2C_UseSkillResponse>
{
    protected override async ETTask Run(Unit unit, C2M_UseSkillRequest request, M2C_UseSkillResponse response)
    {
        SkillDyna dyna = new() { Forward = request.Forward, DstList = request.DstList, DstPosition = request.DstPosition };
        MessageReturn ret = unit.GetComponent<SkillComponent>().UseSKill(request.Id, dyna);
        response.Error = ret.Errno;
        response.Message = ret.Message;
        await ETTask.CompletedTask;
    }
}