using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 护盾伤害加深
    /// <para>万分比</para>
    /// </summary>
    [SubHurt("ShieldAdd")]
    public class ShieldAdd_Hurt: ASubHurt
    {
        public override HurtEffectType GetHurtEffectType()
        {
            return HurtEffectType.HurtBefore;
        }

        public override void Run(FightUnit attack, FightUnit defend, List<int> subArgs, HurtTemp hT, HurtInfo info, List<HurtInfo> hurtInfos)
        {
            
        }
    }
}