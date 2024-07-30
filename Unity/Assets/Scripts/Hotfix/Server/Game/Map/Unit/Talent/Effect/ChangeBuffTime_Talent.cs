using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 修改Buff持续时间
/// <para>修改模式(0累加,1万分比,2替换);参数</para>
/// </summary>
[Talent("ChangeBuffTime")]
public class ChangeBuffTime_Talent: ATalentEffect
{
    private long buffId;
    private int oldMs;
    private long addRoleId;

    public override void Effect(TalentComponent self, TalentUnit talent,
    TalentEffectArgs cfg,
    EffectArgs beEffectCfg,
    Dictionary<string, long> args)
    {
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

        this.oldMs = buff.Ms;
        int d = cfg.Args[1].ToInt();
        switch (cfg.Args[0])
        {
            case "0":
                buff.Ms += d;
                break;
            case "1":
                buff.Ms = (buff.Ms * (1 + d / 10000f)).FloorToInt();
                break;
            case "2":
                buff.Ms = d;
                break;
        }
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

        buff.Ms = this.oldMs;
    }
}