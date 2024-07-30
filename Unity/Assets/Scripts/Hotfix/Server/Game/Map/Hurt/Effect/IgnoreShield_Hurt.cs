using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 无视护盾
    /// <para></para>
    /// </summary>
    [SubHurt("IgnoreShield")]
    public class IgnoreShield_Hurt: ASubHurt
    {
        public override HurtEffectType GetHurtEffectType()
        {
            return HurtEffectType.HurtBefore;
        }

        public override void Run(FightUnit attack, FightUnit defend, List<int> subArgs, HurtTemp hT, HurtInfo info, List<HurtInfo> hurtInfos)
        {
            info.IgnoreShield = true;
        }
    }
}