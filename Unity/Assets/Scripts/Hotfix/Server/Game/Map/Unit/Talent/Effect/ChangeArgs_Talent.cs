using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 参数修改
/// <para>位置;修改模式(0累加,1万分比,2替换);参数</para>
/// </summary>
[Talent("ChangeArgs")]
public class ChangeArgs_Talent: ATalentEffect
{
    public override void Effect(TalentComponent self, TalentUnit talent,
    TalentEffectArgs cfg,
    EffectArgs beEffectCfg,
    Dictionary<string, long> args)
    {
        int pos = cfg.Args[0].ToInt();
        int mode = cfg.Args[1].ToInt();
        int d = cfg.Args[2].ToInt();
        switch (mode)
        {
            case 0:
                beEffectCfg.Args[pos] += d;
                break;
            case 1:
                beEffectCfg.Args[pos] = (beEffectCfg.Args[pos] * (1 + d / 10000f)).FloorToInt();
                break;
            case 2:
                beEffectCfg.Args[pos] = d;
                break;
        }
    }
}