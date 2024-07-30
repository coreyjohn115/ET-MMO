using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 添加指定类型Buff持续时间
/// <para>修改比例(万比);buff类型...;</para>
/// </summary>
[Talent("AddBuffTime")]
public class AddBuffTime_Talent: ATalentEffect
{
    public override void Effect(TalentComponent self, TalentUnit talent,
    TalentEffectArgs cfg,
    EffectArgs beEffectCfg,
    Dictionary<string, long> args)
    {
        int rate = cfg.Args[0].ToInt();
        for (int i = 1; i < cfg.Args.Count; i++)
        {
            int classify = cfg.Args[i].ToInt();
            var dict = self.GetParent<Unit>().GetComponent<BuffComponent>().BuffRateTime;
            if (!dict.TryAdd(classify, rate))
            {
                dict[classify] += rate;
            }
        }
    }

    public override void UnEffect(TalentComponent self, TalentUnit talent)
    {
        int rate = this.EffectArgs.Args[0].ToInt();
        for (int i = 1; i < this.EffectArgs.Args.Count; i++)
        {
            int classify = this.EffectArgs.Args[i].ToInt();
            var dict = self.GetParent<Unit>().GetComponent<BuffComponent>().BuffRateTime;
            if (!dict.ContainsKey(classify))
            {
                continue;
            }

            dict[classify] -= rate;
            if (dict[classify] <= 0)
            {
                dict.Remove(classify);
            }
        }
    }
}