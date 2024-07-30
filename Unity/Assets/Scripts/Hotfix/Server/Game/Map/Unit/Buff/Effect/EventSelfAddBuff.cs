using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 触发指定事件加Buff
/// <para>冷却时间;几率;BuffId;ms;最大人数;目标类型;Buff事件类型...;</para>
/// </summary>
[Buff("EventSelfAddBuff")]
public class EventSelfAddBuff: ABuffEffect
{
    protected override void OnCreate(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
    {
        int buffId = effectArgs.Args[2];
        if (buffId == buff.BuffId)
        {
            Log.Warning($"不允许Buff加自身Buff: {buffId}");
            return;
        }

        var set = new HashSet<int>();
        for (int i = 6; i < effectArgs.Args.Count; i++)
        {
            int kk = effectArgs.Args[i];
            if (kk <= 0)
            {
                continue;
            }

            set.Add(kk);
            self.SubscribeBuffEvent(kk);
        }

        dyna.Args.Add(set);
        dyna.Args.Add(0);
    }
    
    protected override void OnEvent(BuffComponent self, BuffEvent buffEvent, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
    {
        if (dyna.Args[0] is not HashSet<int> set || !set.Contains((int)buffEvent))
        {
            return;
        }

        if (effectArgs.Args[1].IsHit() && dyna.Args[1].ToLong() < TimeInfo.Instance.FrameTime)
        {
            int id = effectArgs.Args[2];
            int ms = effectArgs.Args[3];
            switch (effectArgs.Args[5])
            {
                // 自身
                case 0:
                    self.AddBuff(id, ms);
                    break;
            }

            dyna.Args[1] = TimeInfo.Instance.FrameTime + effectArgs.Args[0];
        }
    }

    protected override void OnRemove(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
    {
        for (int i = 6; i < effectArgs.Args.Count; i++)
        {
            int kk = effectArgs.Args[i];
            if (kk <= 0)
            {
                continue;
            }

            self.UnSubscribeBuffEvent(kk);
        }
    }
}