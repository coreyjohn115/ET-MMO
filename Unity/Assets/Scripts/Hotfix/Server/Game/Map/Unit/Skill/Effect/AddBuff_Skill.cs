using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 添加Buff
/// <para>buffId,概率,时长(毫秒),最大数量</para>
/// </summary>
[Skill(SkillEffectType.AddBuff)]
public class AddBuff_Skill: ASkillEffect
{
    public override HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna)
    {
        int maxCount = this.EffectArgs.Args[3];
        int count = 0;
        foreach (Unit unit in RoleList)
        {
            if (this.EffectArgs.Args[1].IsHit())
            {
                unit.GetComponent<BuffComponent>().AddBuff(this.EffectArgs.Args[0], this.EffectArgs.Args[2], self.Id, (int)skill.Id);
            }

            count++;
            if (count > maxCount)
            {
                break;
            }
        }

        return default;
    }
}