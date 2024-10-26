namespace ET.Server;

[EntitySystemOf(typeof (MapManagerComponent))]
[FriendOf(typeof (MapManagerComponent))]
[FriendOf(typeof (MapUnit))]
public static partial class MapManagerComponentSystem
{
    [EntitySystem]
    private static void Awake(this MapManagerComponent self)
    {
        self.timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(1000, TimerInvokeType.MapCloseCheck, self);
        self.InitMap().NoContext();
    }

    [EntitySystem]
    private static void Destroy(this MapManagerComponent self)
    {
        self.Root().GetComponent<TimerComponent>().Remove(ref self.timer);
    }

    [Invoke(TimerInvokeType.MapCloseCheck)]
    private class MapCloseCheck: ATimer<MapManagerComponent>
    {
        protected override void Run(MapManagerComponent self)
        {
            self.Check();
        }
    }

    public static async ETTask<MapUnit> CreateMapAsync(this MapManagerComponent self, int mapId)
    {
        using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.CreateMap, mapId))
        {
            MapConfig config = MapConfigCategory.Instance.Get2(mapId);
            if (config == default)
            {
                Log.Error($"找不到地图配置: {mapId}");
                return default;
            }

            MapUnit unit = self.AddChild<MapUnit, int>(mapId);
            int id = await FiberManager.Instance.Create(SchedulerType.ThreadPool, self.Zone(), SceneType.Map, "Map");
            var context = await ETTaskHelper.GetContextAsync<ETCancellationToken>();
            if (context.IsCancel())
            {
                unit.Dispose();
                return default;
            }

            unit.fiberId = id;
            unit.actorId = new ActorId(self.Fiber().Process, id);
            if (config.ValidTime > 0)
            {
                unit.closeTime = TimeInfo.Instance.Frame + config.ValidTime * 1000;
            }

            if (!self.mapCfgDict.TryGetValue(mapId, out var list))
            {
                list = [];
                self.mapCfgDict.Add(mapId, list);
            }

            list.Add(unit.Id);
            Log.Console($"地图{unit.Id} {mapId}创建成功, fiberId:{unit.fiberId}");
            return unit;
        }
    }

    public static async ETTask EnterMapAsync(this MapManagerComponent self, long id, int mapId)
    {
        await ETTask.CompletedTask;
    }

    public static async ETTask<(int, ActorId)> GetMapActorId(this MapManagerComponent self, int mapId, long id = 0)
    {
        using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.CreateMap, mapId))
        {
            //指定地图
            if (id > 0)
            {
                MapUnit unit = self.GetChild<MapUnit>(id);
                if (unit == default)
                {
                    return (ErrorCode.ERR_CantFindMap, default);
                }

                return (ErrorCode.ERR_Success, unit.actorId);
            }

            //在当前所在地图中找
            if (self.mapCfgDict.TryGetValue(mapId, out var list))
            {
                foreach (long l in list)
                {
                    MapUnit unit = self.GetChild<MapUnit>(l);
                    if (unit.IsAvailable())
                    {
                        return (ErrorCode.ERR_Success, unit.actorId);
                    }
                }
            }
            else
            {
                //创建新地图
                MapUnit unit = await self.CreateMapAsync(mapId);
                return (ErrorCode.ERR_Success, unit.actorId);
            }

            return (ErrorCode.ERR_LoginError, default);
        }
    }

    private static void Check(this MapManagerComponent self)
    {
        foreach (long id in self.Children.Keys)
        {
            MapUnit unit = self.GetChild<MapUnit>(id);
            if (unit.closeTime > 0 && TimeInfo.Instance.Frame >= unit.closeTime)
            {
                //15S后销毁
                unit.validTime = unit.closeTime + 15000;
                Log.Console($"地图{unit.Id} {unit.mapId}关闭");
            }

            if (unit.validTime > 0 && TimeInfo.Instance.Frame >= unit.validTime)
            {
                self.Remove(unit.Id);
            }
        }
    }

    private static void Remove(this MapManagerComponent self, long id)
    {
        MapUnit unit = self.GetChild<MapUnit>(id);
        FiberManager.Instance.Remove(unit.fiberId).NoContext();
        unit.Dispose();
        if (self.mapCfgDict.TryGetValue(unit.mapId, out var list))
        {
            list.Remove(id);
        }

        Log.Console($"地图{unit.Id} {unit.mapId}销毁");
    }

    private static async ETTask InitMap(this MapManagerComponent self)
    {
        foreach (var pair in MapConfigCategory.Instance.GetAll())
        {
            if (pair.Value.Auto)
            {
                await self.CreateMapAsync(pair.Key);
            }
        }
    }
}