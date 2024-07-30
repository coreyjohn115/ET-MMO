using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 属性修改属性
    /// <para>要修改的类型,目标类型,发挥比例,附加值</para>
    /// </summary>
    [SubHurt("AttrChangeAttr")]
    public class AttrChangeAttr_Hurt: ASubHurt
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