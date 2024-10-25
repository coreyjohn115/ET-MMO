using System;
using System.Collections.Generic;
using System.Linq;

namespace ET.Server
{
    [EntitySystemOf(typeof (CacheComponent))]
    [FriendOf(typeof (CacheComponent))]
    public static partial class CacheComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CacheComponent self)
        {
            self.cacheDict.Clear();
            self.LoadCacheKey();
            self.checkExpireTimer = self.Scene().GetComponent<TimerComponent>().NewRepeatedTimer(10 * 1000, TimerInvokeType.CacheCheck, self);
            Log.Info($"缓存服务器启动完成：{self.Zone()}");
        }

        [EntitySystem]
        private static void Destroy(this CacheComponent self)
        {
            self.Scene().GetComponent<TimerComponent>().Remove(ref self.checkExpireTimer);
            self.cacheKeyList.Clear();
            foreach (UnitCache unitCache in self.cacheDict.Values)
            {
                unitCache.Dispose();
            }

            self.cacheDict.Clear();
        }

        [EntitySystem]
        private static void Load(this CacheComponent self)
        {
            self.Scene().GetComponent<TimerComponent>().Remove(ref self.checkExpireTimer);
            self.checkExpireTimer = self.Scene().GetComponent<TimerComponent>().NewRepeatedTimer(10 * 1000, TimerInvokeType.CacheCheck, self);
        }

        [Invoke(TimerInvokeType.CacheCheck)]
        private class CacheCheckExpire: ATimer<CacheComponent>
        {
            protected override void Run(CacheComponent self)
            {
                if (self.IsClearData())
                {
                    return;
                }

                self.Check().NoContext();
            }
        }

        private static async ETTask Check(this CacheComponent self)
        {
            //需要保存的数据
            if (self.needSaveDict.Count > 0)
            {
                var dict = self.needSaveDict.ToDictionary(x => x.Key, x => x.Value);
                self.needSaveDict.Clear();
                using ListComponent<ETTask> taskList = ListComponent<ETTask>.Create();
                foreach ((long id, var list) in dict)
                {
                    ETTask t = self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Save(id, list);
                    taskList.Add(t);
                }

                await ETTaskHelper.WaitAll(taskList);
            }

            //检测过期的数据
            foreach (UnitCache unitCache in self.cacheDict.Values)
            {
                try
                {
                    unitCache.Check();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        private static void LoadCacheKey(this CacheComponent self)
        {
            self.cacheKeyList.Clear();
            foreach (Type type in CodeTypes.Instance.GetTypes().Values)
            {
                if (!type.IsAbstract && typeof (ICache).IsAssignableFrom(type))
                {
                    self.cacheKeyList.Add(type.FullName);
                }
            }

            foreach (string s in self.cacheKeyList)
            {
                UnitCache unitCache = self.AddChild<UnitCache>();
                unitCache.TypeName = s;
                self.cacheDict.Add(s, unitCache);
            }
        }

        public static async ETTask<Entity> Get(this CacheComponent self, long id, string key)
        {
            if (self.cacheDict.TryGetValue(key, out var unitCache))
            {
                return await unitCache.Get(id);
            }

            unitCache = self.AddChild<UnitCache>();
            unitCache.TypeName = key;
            self.cacheDict.Add(key, unitCache);

            return await unitCache.Get(id);
        }

        public static void UpdateCache(this CacheComponent self, long id, ListComponent<Entity> listComponent)
        {
            foreach (Entity entity in listComponent)
            {
                string name = entity.GetType().FullName;
                if (!self.cacheDict.TryGetValue(name, out UnitCache uniCache))
                {
                    uniCache = self.AddChild<UnitCache>();
                    uniCache.TypeName = name;
                    self.cacheDict.Add(name, uniCache);
                }

                uniCache.AddOrUpdate(entity);
                if (!self.needSaveDict.TryGetValue(id, out var list))
                {
                    list = new List<Entity>(10);
                    self.needSaveDict.Add(id, list);
                }

                list.Add(entity);
            }
        }

        public static void DeleteCache(this CacheComponent self, long id)
        {
            foreach (UnitCache unitCache in self.cacheDict.Values)
            {
                unitCache.Delete(id);
            }
        }

        public static async ETTask Save(this CacheComponent self)
        {
            await self.Check();
        }
    }
}