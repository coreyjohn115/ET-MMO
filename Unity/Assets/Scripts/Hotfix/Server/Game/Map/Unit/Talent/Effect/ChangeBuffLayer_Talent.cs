using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 修改buff 最大叠层
/// <para>最大叠层</para>
/// </summary>
[Talent("ChangeBuffLayer")]
public class ChangeBuffLayer_Talent: ATalentEffect
{
    private long buffId;
    private int oldLayer;
    private long addRoleId;

    public override void Effect(TalentComponent self, TalentUnit talent,
    TalentEffectArgs cfg,
    EffectArgs beEffectCfg,
    Dictionary<string, long> args)
    {
        int maxLayer = cfg.Args[0].ToInt();
        if (!args.TryGetValue("Id", out long id))
        {
            return;
        }

        buffId = id;
        addRoleId = args["AddRoleId"];
        Unit addUnit = self.Scene().GetComponent<UnitComponent>().Get(addRoleId);
        BuffUnit buff = addUnit.GetComponent<BuffComponent>().GetBuff(id);
        if (!buff)
        {
            return;
        }

        this.oldLayer = buff.MaxLayer;
        buff.MaxLayer = maxLayer;
    }

    public override void UnEffect(TalentComponent self, TalentUnit talent)
    {
        Unit addUnit = self.Scene().GetComponent<UnitComponent>().Get(addRoleId);
        BuffUnit buff = addUnit.GetComponent<BuffComponent>().GetBuff(this.buffId);
        if (!buff)
        {
            return;
        }

        buff.MaxLayer = this.oldLayer;
    }
}