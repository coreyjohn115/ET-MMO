using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 修改属性
/// <para>属性类型;属性值...;</para>
/// </summary>
[Talent("ChangeAttr")]
public class ChangeAttr_Talent: ATalentEffect
{
    private static void Add(Unit unit, bool isAdd, List<string> effect)
    {
        for (int i = 0; i < effect.Count / 2; i++)
        {
            int attrType = effect[i * 2].ToInt();
            if (attrType <= 0)
            {
                continue;
            }

            int iv = effect[i * 2 + 1].ToInt();
            int attrValue = isAdd? iv : -iv;
            unit.GetComponent<NumericComponent>().Add(attrType, attrValue);
        }
    }

    public override void Effect(TalentComponent self, TalentUnit talent,
    TalentEffectArgs cfg,
    EffectArgs beEffectCfg,
    Dictionary<string, long> args)
    {
        Add(self.GetParent<Unit>(), true, cfg.Args);
    }

    public override void UnEffect(TalentComponent self, TalentUnit talent)
    {
        Add(self.GetParent<Unit>(), false, this.EffectArgs.Args);
    }
}