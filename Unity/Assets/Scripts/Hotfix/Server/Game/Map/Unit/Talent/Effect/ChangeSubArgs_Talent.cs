using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 修改子效果参数
/// <para>子效果名称;位置;修改模式(0累加,1万分比,2替换);参数</para>
/// </summary>
[Talent("ChangeSubArgs")]
public class ChangeSubArgs_Talent: ATalentEffect
{
    public override void Effect(TalentComponent self, TalentUnit talent,
    TalentEffectArgs cfg,
    EffectArgs beEffectCfg,
    Dictionary<string, long> args)
    {
        int pos = cfg.Args[1].ToInt();
        int d = cfg.Args[3].ToInt();
        foreach (SubEffectArgs sub in beEffectCfg.SubList)
        {
            if (sub.Cmd == cfg.Args[0])
            {
                switch (cfg.Args[2])
                {
                    case "0":
                        sub.Args[pos] += d;
                        break;
                    case "1":
                        sub.Args[pos] = (sub.Args[pos] * (1 + d / 10000f)).FloorToInt();
                        break;
                    case "2":
                        sub.Args[pos] = d;
                        break;
                }
            }
        }
    }
}