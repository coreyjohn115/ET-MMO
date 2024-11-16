using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 移除buff
/// buffId,移除层数
/// </summary>
[Skill(SkillEffectType.RemoveBuffById)]
public class RemoveBuffById_Skill: ASkillEffect
{
    public override HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna)
    {
        foreach (Unit unit in RoleList)
        {
            unit.GetComponent<BuffComponent>().RemoveBuffLayer(this.EffectArgs.Args[0], this.EffectArgs.Args.Get(1, 1));
        }

        return default;
    }
}