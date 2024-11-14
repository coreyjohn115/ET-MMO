using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 治疗
    /// 技能发挥比例,附攻,最大数量
    /// </summary>
    [Skill(SkillEffectType.Heal)]
    public class Heal_Skill: ASkillEffect
    {
        public override HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna)
        {
            return default;
        }
    }
}