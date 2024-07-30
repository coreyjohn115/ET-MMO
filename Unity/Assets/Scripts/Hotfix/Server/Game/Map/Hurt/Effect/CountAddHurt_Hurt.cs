using System;
using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 数量附加伤害
    /// <para>每人增加发挥比例;最大比例;</para>
    /// </summary>
    [SubHurt("CountAddHurt")]
    public class CountAddHurt_Hurt: ASubHurt
    {
        public override HurtEffectType GetHurtEffectType()
        {
            return HurtEffectType.EffectAfter;
        }

        public override void Run(FightUnit attack, FightUnit defend, List<int> subArgs, HurtTemp hT, HurtInfo info, List<HurtInfo> hurtInfos)
        {
            int addRate = Math.Min(hT.objectList.Count * subArgs[0], subArgs[1]);
            foreach (HurtInfo hurtInfo in hurtInfos)
            {
                float rate = 1 + addRate / 10000f;
                hurtInfo.Proto.Hurt = (hurtInfo.Proto.Hurt * rate).Ceil();
                hurtInfo.OriginHurt = hurtInfo.Proto.Hurt;
            }
        }
    }
}