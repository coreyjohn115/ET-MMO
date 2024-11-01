using System;

namespace ET.Server
{
    public static class CacheHelper
    {
        private static async ETTask<Unit> GetCache(Scene scene, long unitId)
        {
            var cacheCfg = StartSceneConfigCategory.Instance.GetCache(scene.Zone());
            Other2Cache_GetCache proto = Other2Cache_GetCache.Create();
            proto.UnitId = unitId;
            var resp = (Cache2Other_GetCache)await scene.GetComponent<MessageSender>().Call(cacheCfg.ActorId, proto);
            if (resp.Error != ErrorCode.ERR_Success || resp.Entitys.IsNullOrEmpty())
            {
                return default;
            }

            int index = resp.ComponentNameList.IndexOf(typeof (Unit).FullName);
            if (index == -1)
            {
                return default;
            }

            if (resp.Entitys[index] == null)
            {
                return default;
            }

            var unit = MongoHelper.Deserialize<Unit>(resp.Entitys[index]);
            if (unit == null)
            {
                return default;
            }

            scene.GetComponent<UnitComponent>().AddChild(unit);
            scene.GetComponent<UnitComponent>().Add(unit);
            foreach (var bytes in resp.Entitys)
            {
                if (bytes == null)
                {
                    continue;
                }

                Entity entity = MongoHelper.Deserialize<Entity>(bytes);
                if (entity == unit)
                {
                    continue;
                }

                unit.AddComponent(entity);
            }

            return unit;
        }
        
        public static async ETTask<(bool, Unit)> LoadUnit(Player player)
        {
            Unit unit = await GetCache(player.Scene(), player.Id);
            bool isNewPlayer = unit == null;
            if (isNewPlayer)
            {
                unit = UnitFactory.Create(player.Scene(), player.Id, UnitType.Player);
            }

            return (isNewPlayer, unit);
        }

        public static async ETTask InitUnit(Unit unit, Player player, bool isNewPlayer)
        {
            foreach ((string name, string fullName) in UnitComponentSingleton.Instance.GetUnitComs())
            {
                if (unit.GetComponentByName(name) != null)
                {
                    continue;
                }

                Type t = CodeTypes.Instance.GetType(fullName);
                unit.AddComponent(t);
                isNewPlayer = true;
            }
            
            if (isNewPlayer)
            {
                unit.GetComponent<UnitBasic>().Initialize(player);
                UpdateAllCache(unit.Scene(), unit);
            }

            await ETTask.CompletedTask;
        }

        
        public static async ETTask UpdateCache<T>(this T self) where T : Entity, ICache
        {
            var cacheCfg = StartSceneConfigCategory.Instance.GetCache(self.Zone());
            Other2Cache_UpdateCache message = Other2Cache_UpdateCache.Create();
            message.UnitId = self.Id;
            message.EntityTypeList.Add(self.GetType().FullName);
            message.EntityData.Add(self.ToBson());
            await self.Scene().GetComponent<MessageSender>().Call(cacheCfg.ActorId, message);
        }

        public static void UpdateAllCache(Scene scene, Unit unit)
        {
            var cacheCfg = StartSceneConfigCategory.Instance.GetCache(unit.Zone());
            Other2Cache_UpdateCache message = Other2Cache_UpdateCache.Create();
            message.UnitId = unit.Id;
            message.EntityTypeList.Add(unit.GetType().FullName);
            message.EntityData.Add(unit.ToBson());

            foreach ((long id, Entity entity) in unit.Components)
            {
                var t = entity.GetType();
                if (!typeof (ICache).IsAssignableFrom(t))
                {
                    continue;
                }

                message.EntityTypeList.Add(t.FullName);
                message.EntityData.Add(entity.ToBson());
            }

            scene.GetComponent<MessageSender>().Send(cacheCfg.ActorId, message);
        }

        public static async ETTask<T> GetComponentCache<T>(this T self) where T : Entity, ICache
        {
            var cacheCfg = StartSceneConfigCategory.Instance.GetCache(self.Zone());
            Other2Cache_GetCache message = Other2Cache_GetCache.Create();
            message.UnitId = self.Id;
            message.ComponentNameList.Add(typeof (T).FullName);
            var resp = (Cache2Other_GetCache)await self.Scene().GetComponent<MessageSender>().Call(cacheCfg.ActorId, message);
            if (resp.Error == ErrorCode.ERR_Success && resp.Entitys.Count > 0)
            {
                return MongoHelper.Deserialize<T>(resp.Entitys[0]);
            }

            return default;
        }

        /// <summary>
        /// 查询玩家简要信息
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public static async ETTask<PlayerInfoProto> GetPlayerInfo(Entity self, long unitId)
        {
            var cacheCfg = StartSceneConfigCategory.Instance.GetCache(self.Zone());
            Other2Cache_GetCache message = Other2Cache_GetCache.Create();
            message.UnitId = unitId;
            message.ComponentNameList.Add(typeof (UnitBasic).FullName);
            var resp = (Cache2Other_GetCache)await self.Scene().GetComponent<MessageSender>().Call(cacheCfg.ActorId, message);
            if (resp.Error == ErrorCode.ERR_Success && resp.Entitys.Count > 0)
            {
                UnitBasic extra = MongoHelper.Deserialize<UnitBasic>(resp.Entitys[0]);
                return extra.ToPlayerInfo();
            }

            return default;
        }

        public static async ETTask DeleteCache(Scene scene, long unitId)
        {
            var cacheCfg = StartSceneConfigCategory.Instance.GetCache(scene.Zone());
            Other2Cache_DeleteCache proto = Other2Cache_DeleteCache.Create();
            proto.UnitId = unitId;
            await scene.GetComponent<MessageSender>().Call(cacheCfg.ActorId, proto);
        }
    }
}