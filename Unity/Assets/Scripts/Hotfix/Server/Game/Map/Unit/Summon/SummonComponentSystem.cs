using System.Collections.Generic;

namespace ET.Server;

[EntitySystemOf(typeof (SummonComponent))]
public static partial class SummonComponentSystem
{
    [Invoke(TimerInvokeType.SummonCheck)]
    private class SummonCheckTimer: ATimer<SummonComponent>
    {
        protected override void Run(SummonComponent self)
        {
            self.Check();
        }
    }

    [EntitySystem]
    private static void Awake(this SummonComponent self)
    {
        self.timer = self.Scene().GetComponent<TimerComponent>().NewRepeatedTimer(500L, TimerInvokeType.SummonCheck, self);
    }

    [EntitySystem]
    private static void Destroy(this SummonComponent self)
    {
        self.Scene().GetComponent<TimerComponent>().Remove(ref self.timer);
        UnitComponent unitComponent = self.Scene().GetComponent<UnitComponent>();
        foreach (long id in self.summonDict.Keys)
        {
            unitComponent.Remove(id);
        }

        self.summonDict.Clear();
    }

    public static void Summon(this SummonComponent self, int configId, int count, int aliveTime = 0, CampType? camp = default)
    {
        Unit unit = self.GetParent<Unit>();
        camp ??= unit.Camp;

        ObjectConfig config = ObjectConfigCategory.Instance.Get2(configId);
        long validTime = aliveTime > 0? aliveTime : config.AliveTime;
        for (int i = 0; i < count; i++)
        {
            Unit summon = UnitFactory.Create(self.Scene(), IdGenerater.Instance.GenerateId(), UnitType.Summon, configId);
            summon.GetComponent<BelongComponent>().SetBelong(unit.Id);
            summon.Camp = camp.Value;
            self.summonDict.Add(summon.Id, validTime);
        }
    }

    private static void Check(this SummonComponent self)
    {
        if (self.summonDict.Count <= 0)
        {
            return;
        }

        UnitComponent unitComponent = self.Scene().GetComponent<UnitComponent>();
        using ListComponent<long> list = ListComponent<long>.Create();
        foreach (KeyValuePair<long, long> pair in self.summonDict)
        {
            if (!unitComponent.Get(pair.Key))
            {
                list.Add(pair.Key);
                continue;
            }

            if (pair.Value > 0 && TimeInfo.Instance.Frame >= pair.Value)
            {
                list.Add(pair.Key);
            }
        }

        foreach (long l in list)
        {
            self.Scene().GetComponent<UnitComponent>().Remove(l);
            self.summonDict.Remove(l);
        }
    }
}