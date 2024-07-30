using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 逐层移除Buff
    /// BuffId,移除层数
    /// </summary>
    [Skill("RemoveBuffLayer")]
    public class RemoveBuffLayer_Skill: ASkillEffect
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
}