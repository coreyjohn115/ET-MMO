using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 添加Buff
/// <para>buffId;时间(ms)</para>
/// </summary>
[Talent("AddBuff")]
public class AddBuff_Talent: ATalentEffect
{
    public override void Effect(TalentComponent self, TalentUnit talent,
    TalentEffectArgs cfg,
    EffectArgs beEffectCfg,
    Dictionary<string, long> args)
    {
        int buffId = cfg.Args[0].ToInt();
        int ms = cfg.Args[1].ToInt();
        self.GetParent<Unit>().GetComponent<BuffComponent>().AddBuffExt(buffId, 1, ms);
    }

    public override void UnEffect(TalentComponent self, TalentUnit talent)
    {
        int buffId = this.EffectArgs.Args[0].ToInt();
        self.GetParent<Unit>().GetComponent<BuffComponent>().RemoveBuff(buffId);
    }
}