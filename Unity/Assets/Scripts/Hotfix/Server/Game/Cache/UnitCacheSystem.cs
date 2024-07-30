namespace ET.Server
{
    [FriendOf(typeof (UnitCache))]
    [EntitySystemOf(typeof (UnitCache))]
    public static partial class UnitCacheSystem
    {
        [EntitySystem]
        private static void Awake(this UnitCache self)
        {
        }

        [EntitySystem]
        private static void Destroy(this UnitCache self)
        {
            foreach (Entity entity in self.componentDict.Values)
            {
                entity.Dispose();
            }

            self.updateTimeDict.Clear();
            self.componentDict.Clear();
            self.TypeName = null;
        }

        /// <summary>
        /// 检测缓存
        /// </summary>
        /// <param name="self"></param>
        public static void Check(this UnitCache self)
        {
            using ListComponent<long> ids = ListComponent<long>.Create();
            ids.AddRange(self.componentDict.Keys);
            foreach (long id in ids)
            {
                if (!self.componentDict.TryGetValue(id, out Entity entity))
                {
                    continue;
                }

                if (TimeInfo.Instance.FrameTime - self.updateTimeDict.Get(id) <= UnitCache.Interval)
                {
                    continue;
                }

                entity.Dispose();
                self.componentDict.Remove(id);
                self.updateTimeDict.Remove(id);
            }
        }

        public static void AddOrUpdate(this UnitCache self, Entity entity)
        {
            if (entity == null)
            {
                return;
            }

            if (self.componentDict.TryGetValue(entity.Id, out var old))
            {
                if (old != entity)
                {
                    old.Dispose();
                }

                self.updateTimeDict.Remove(entity.Id);
                self.componentDict.Remove(entity.Id);
            }

            self.componentDict.Add(entity.Id, entity);
            self.updateTimeDict.Add(entity.Id, TimeInfo.Instance.FrameTime);
        }

        public static async ETTask<Entity> Get(this UnitCache self, long id)
        {
            if (self.componentDict.TryGetValue(id, out var entity))
            {
                self.updateTimeDict[entity.Id] = TimeInfo.Instance.FrameTime;
                return entity;
            }

            entity = await self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Query<Entity>(id, self.TypeName);
            self.AddOrUpdate(entity);

            return entity;
        }

        public static void Delete(this UnitCache self, long id)
        {
            if (!self.componentDict.TryGetValue(id, out var cache))
            {
                return;
            }

            cache.Dispose();
            self.componentDict.Remove(id);
            self.updateTimeDict.Remove(id);
        }
    }
}