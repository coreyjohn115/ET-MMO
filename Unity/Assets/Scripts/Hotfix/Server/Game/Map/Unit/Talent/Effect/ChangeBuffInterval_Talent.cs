using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 修改buff间隔时间
/// <para>间隔时间</para>
/// </summary>
[Talent("ChangeBuffInterval")]
public class ChangeBuffInterval_Talent: ATalentEffect
{
    private long buffId;
    private int oldInterval;
    private long addRoleId;

    public override void Effect(TalentComponent self, TalentUnit talent,
    TalentEffectArgs cfg,
    EffectArgs beEffectCfg,
    Dictionary<string, long> args)
    {
        int interval = cfg.Args[0].ToInt();
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

        this.oldInterval = buff.Interval;
        buff.Interval = interval;
    }

    public override void UnEffect(TalentComponent self, TalentUnit talent)
    {
        Unit addUnit = self.Scene().GetComponent<UnitComponent>().Get(addRoleId);
        if (!addUnit)
        {
            return;
        }

        BuffUnit buff = addUnit.GetComponent<BuffComponent>().GetBuff(this.buffId);
        if (!buff)
        {
            return;
        }

        buff.Interval = this.oldInterval;
    }
}