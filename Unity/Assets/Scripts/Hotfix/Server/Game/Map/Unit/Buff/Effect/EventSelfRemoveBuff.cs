using System.Collections.Generic;

namespace ET.Server;

/// <summary>
/// 触发指定事件删除Buff
/// <para>BuffId;Buff事件类型...;</para>
/// </summary>
[Buff("EventSelfRemoveBuff")]
public class EventSelfRemoveBuff: ABuffEffect
{
    protected override void OnCreate(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
    {
        var set = new HashSet<int>();
        for (int i = 1; i < effectArgs.Args.Count; i++)
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
    }

    protected override void OnEvent(BuffComponent self, BuffEvent buffEvent, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
    {
        if (dyna.Args[0] is HashSet<int> set && set.Contains((int)buffEvent))
        {
            self.RemoveBuff(effectArgs.Args[0]);
        }
    }

    protected override void OnRemove(BuffComponent self, BuffUnit buff, BuffDyna dyna, EffectArgs effectArgs, object[] args)
    {
        for (int i = 1; i < effectArgs.Args.Count; i++)
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