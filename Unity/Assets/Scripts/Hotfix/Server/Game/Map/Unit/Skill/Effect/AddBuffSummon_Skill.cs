using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 添加buff（地块魔法）
    /// buffId,持续时间,对象Id
    /// </summary>
    [Skill("AddBuffSummon")]
    public class AddBuffSummon_Skill: ASkillEffect
    {
        public override HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna)
        {
            return default;
        }
    }
}