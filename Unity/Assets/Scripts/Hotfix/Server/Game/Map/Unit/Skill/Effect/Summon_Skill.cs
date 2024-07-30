using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 召唤
    /// 对象ID,数量,阵营
    /// </summary>
    [Skill("Summon")]
    public class Summon_Skill: ASkillEffect
    {
        public override HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna)
        {
            return default;
        }
    }
}