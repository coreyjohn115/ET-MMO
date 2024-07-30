using System;
using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 暴击返还CD
    /// <para>返还的时间(Ms);</para>
    /// </summary>
    [SubHurt("CritBackCd")]
    public class CritBackCd_Hurt: ASubHurt
    {
        public override HurtEffectType GetHurtEffectType()
        {
            return HurtEffectType.HurtCrit;
        }

        public override void Run(FightUnit attack, FightUnit defend, List<int> subArgs, HurtTemp hT, HurtInfo info, List<HurtInfo> hurtInfos)
        {
            Unit unit = attack.Scene().GetComponent<UnitComponent>().Get(attack.Id);
            if (!unit)
            {
                return;
            }

            unit.GetComponent<SkillComponent>().AddSkillCd(hT.id, -subArgs[0]);
        }
    }
}