using System.Collections.Generic;

namespace ET;

public partial class TalentConfig
{
    public Dictionary<int, Dictionary<int, List<TalentEffectArgs>>> DstMap { get; private set; }

    public override void EndInit()
    {
        DstMap = [];
        foreach (TalentEffectArgs effectArg in this.EffectList)
        {
            foreach (OftCfgs oftCfg in effectArg.OftList)
            {
                foreach (int id in oftCfg.DstList)
                {
                    if (!DstMap.TryGetValue(id, out var dict1))
                    {
                        dict1 = [];
                        DstMap.Add(id, dict1);
                    }

                    if (!dict1.TryGetValue(oftCfg.Idx, out var list))
                    {
                        list = [];
                    }

                    list.Add(effectArg);
                }
            }
        }
    }
}