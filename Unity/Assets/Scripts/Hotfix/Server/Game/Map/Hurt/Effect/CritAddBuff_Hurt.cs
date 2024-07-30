using System;
using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 暴击添加buff
    /// <para>BuffId;时间(ms)</para>
    /// </summary>
    [SubHurt("CritAddBuff")]
    public class CritAddBuff_Hurt: ASubHurt
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

            unit.GetComponent<BuffComponent>().AddBuff(subArgs[0], subArgs[1]);
        }
    }
}