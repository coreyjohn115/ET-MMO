using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 伤害
/// <para>技能发挥比例,额外附加攻击,最大数量,打断技能类型</para>
/// </summary>
[Skill(SkillEffectType.Hurt)]
public class Hurt_Skill: ASkillEffect
{
    public override HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna)
    {
        var pkg = new HurtPkg() { ViewCmd = this.EffectArgs.ViewCmd };
        pkg.HurtInfos = HurtHelper.StandHurt(self.GetParent<Unit>(),
            RoleList,
            (int)skill.Id,
            this.EffectArgs.Args[0],
            this.EffectArgs.Args[1],
            this.EffectArgs.Args[2],
            skill.MasterConfig.HateBase,
            skill.MasterConfig.HateRate,
            this.EffectArgs.SubList,
            dyna,
            skill.MasterConfig.Element);
        return pkg;
    }
}