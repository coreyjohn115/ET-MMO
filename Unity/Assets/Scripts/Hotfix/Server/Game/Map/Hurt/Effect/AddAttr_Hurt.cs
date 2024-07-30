using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 本次伤害添加属性
    /// <para>属性类型,属性值...</para>
    /// </summary>
    [SubHurt("AddAttr")]
    public class AddAttr_Hurt: ASubHurt
    {
        public override HurtEffectType GetHurtEffectType()
        {
            return HurtEffectType.EffectBefore;
        }

        public override void Run(FightUnit attack, FightUnit defend, List<int> subArgs, HurtTemp hT, HurtInfo info, List<HurtInfo> hurtInfos)
        {
            for (int i = 0; i < subArgs.Count / 2; i++)
            {
                int t = subArgs[i * 2];
                int v = subArgs[i * 2 + 1];
                attack.NumericDic[t] += v;
            }
        }
    }
}