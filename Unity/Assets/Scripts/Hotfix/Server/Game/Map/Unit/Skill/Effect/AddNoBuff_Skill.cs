using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 添加buff时若目标没有某buff则额外添加x层该buff
    /// <para>BuffId,概率,持续时间,最大数量,目标BuffId,额外层数</para>
    /// </summary>
    [Skill(SkillEffectType.AddNoBuff)]
    public class AddNoBuff_Skill: ASkillEffect
    {
        public override HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna)
        {
            int buffId = this.EffectArgs.Args[0];
            int maxCount = this.EffectArgs.Args[3];
            int count = 0;
            foreach (Unit unit in RoleList)
            {
                BuffComponent buffCom = unit.GetComponent<BuffComponent>();
                int layer = 1;
                if (!buffCom.GetBuff(buffId))
                {
                    layer = this.EffectArgs.Args[5];
                }

                if (this.EffectArgs.Args[1].IsHit())
                {
                    buffCom.AddBuff(this.EffectArgs.Args[4], this.EffectArgs.Args[2], self.Id, (int)skill.Id, layer);
                }

                count++;
                if (count >= maxCount)
                {
                    break;
                }
            }

            return default;
        }
    }
}