using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 添加子效果
/// <para>子效果名称;效果参数...;</para>
/// </summary>
[Talent("AddSub")]
public class AddSub_Talent: ATalentEffect
{
    public override void Effect(TalentComponent self, TalentUnit talent,
    TalentEffectArgs cfg,
    EffectArgs beEffectCfg,
    Dictionary<string, long> args)
    {
        var subInfo = new SubEffectArgs();
        subInfo.Cmd = cfg.Args[0];
        for (int i = 1; i < cfg.Args.Count; i++)
        {
            subInfo.Args.Add(cfg.Args[i].ToInt());
        }

        beEffectCfg.SubList.Add(subInfo);
    }
}