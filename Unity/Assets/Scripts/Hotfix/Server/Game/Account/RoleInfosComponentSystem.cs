using System.Collections.Generic;
using MongoDB.Driver;

namespace ET.Server
{
    [EntitySystemOf(typeof (RoleInfosComponent))]
    public static partial class RoleInfosComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RoleInfosComponent self)
        {
            var collection = self.Scene().GetComponent<DBManagerComponent>().GetDB().GetCollection<RoleInfo>();
            collection.Indexes.CreateMany(new[]
            {
                new CreateIndexModel<RoleInfo>(Builders<RoleInfo>.IndexKeys.Ascending(info => info.Name)),
                new CreateIndexModel<RoleInfo>(Builders<RoleInfo>.IndexKeys.Ascending(info => info.Account)),
                new CreateIndexModel<RoleInfo>(Builders<RoleInfo>.IndexKeys.Ascending(info => info.ServerId)),
                new CreateIndexModel<RoleInfo>(Builders<RoleInfo>.IndexKeys.Ascending(info => info.State)),
            });
        }

        [EntitySystem]
        private static void Destroy(this RoleInfosComponent self)
        {
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="self"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async ETTask<List<RoleInfo>> GetRoleList(this RoleInfosComponent self, string account)
        {
            var db = self.Scene().GetComponent<DBManagerComponent>().GetDB();
            using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.Account, account.GetLongHashCode()))
            {
                List<RoleInfo> roles = await db.Query<RoleInfo>(info => info.Account == account && info.State == (int)RoleInfoState.Normal);
                return roles;
            }
        }
    }
}