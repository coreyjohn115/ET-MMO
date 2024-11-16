using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 治疗
/// 技能发挥比例,附攻,最大数量
/// </summary>
[Skill(SkillEffectType.Heal)]
public class Heal_Skill: ASkillEffect
{
    public override HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna)
    {
        var pkg = new HurtPkg() { ViewCmd = this.EffectArgs.ViewCmd };
        foreach (Unit target in RoleList)
        {
            var info = HurtHelper.Heal(self.GetParent<Unit>(), target, this.EffectArgs.Args[0], this.EffectArgs.Args[1]);
            pkg.HurtInfos.Add(info);
            target.AddHp(info.Hurt, self.Id, (int)skill.Id);
        }

        return pkg;
    }
}