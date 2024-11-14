using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 重置buff持续时间
    /// <para>BuffID</para>
    /// </summary>
    [Skill(SkillEffectType.ResetBuffTime)]
    public class ResetBuffTime_Skill: ASkillEffect
    {
        public override HurtPkg Run(SkillComponent self, SkillUnit skill, HashSet<Unit> RoleList, SkillDyna dyna)
        {
            foreach (Unit unit in RoleList)
            {
                BuffUnit buff = unit.GetComponent<BuffComponent>().GetBuff(this.EffectArgs.Args[0]);
                if (!buff)
                {
                    continue;
                }

                buff.ValidTime = TimeInfo.Instance.Frame + buff.Ms;
                unit.GetComponent<BuffComponent>().SyncBuffToClient(buff).NoContext();
            }

            return default;
        }
    }
}