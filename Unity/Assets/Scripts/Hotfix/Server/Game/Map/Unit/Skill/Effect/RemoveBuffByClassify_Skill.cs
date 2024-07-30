using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 移除类型buff
    /// <para>buff类型,移除层数</para>
    /// </summary>
    [Skill("RemoveBuffByClassify")]
    public class RemoveBuffByClassify_Skill: ASkillEffect
    {
        public override HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna)
        {
            foreach (Unit unit in RoleList)
            {
                unit.GetComponent<BuffComponent>().RemoveBuffByClassify(this.EffectArgs.Args[0], this.EffectArgs.Args.Get(1, 1));
            }

            return default;
        }
    }
}