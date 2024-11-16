using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 召唤
/// 对象ID,数量,存活时间,阵营
/// </summary>
[Skill(SkillEffectType.Summon)]
public class Summon_Skill: ASkillEffect
{
    public override HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna)
    {
        Unit unit = self.GetParent<Unit>();
        int aliveTime = this.EffectArgs.Args.Get(2);
        CampType? campType = null;
        if (this.EffectArgs.Args.Count > 3)
        {
            campType = (CampType)this.EffectArgs.Args[3];
        }

        unit.GetComponent<SummonComponent>().Summon(this.EffectArgs.Args[0], this.EffectArgs.Args[1], aliveTime, campType);
        return default;
    }
}