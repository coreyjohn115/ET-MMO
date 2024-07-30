using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 增加伤害
    /// <para>是否生效;增加比例(万分比);</para>
    /// </summary>
    [SubHurt("AddHurt")]
    public class AddHurt_Hurt: ASubHurt
    {
        public override HurtEffectType GetHurtEffectType()
        {
            return HurtEffectType.EffectAfter;
        }

        public override void Run(FightUnit attack, FightUnit defend, List<int> subArgs, HurtTemp hT, HurtInfo info, List<HurtInfo> hurtInfos)
        {
            foreach (HurtInfo hurtInfo in hurtInfos)
            {
                long hurtV = (subArgs[1] / 10000f * hurtInfo.OriginHurt).Ceil();
                hurtInfo.Proto.Hurt = hurtV;
                hurtInfo.OriginHurt = hurtV;
            }
        }
    }
}