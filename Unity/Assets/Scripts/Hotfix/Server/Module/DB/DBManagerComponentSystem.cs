using System;

namespace ET.Server
{
    [FriendOf(typeof (DBManagerComponent))]
    public static class DBManagerComponentSystem
    {
        /// <summary>
        /// 清档
        /// </summary>
        /// <param name="self"></param>
        /// <param name="zone"></param>
        public static async ETTask Clear(this DBManagerComponent self, int zone)
        {
            DBComponent zoneDB = self.GetZoneDB(zone);
            await zoneDB.DropDatabase();
        }

        public static DBComponent GetZoneDB(this DBManagerComponent self, int zone)
        {
            DBComponent dbComponent = self.GetChild<DBComponent>(zone);
            if (dbComponent)
            {
                return dbComponent;
            }

            StartZoneConfig startZoneConfig = StartZoneConfigCategory.Instance.Get(zone);
            if (startZoneConfig.DBConnection == "")
            {
                throw new Exception($"common db not found mongo connect string");
            }

            dbComponent = self.AddChildWithId<DBComponent, string, string>(zone, startZoneConfig.DBConnection, startZoneConfig.DBName);
            return dbComponent;
        }

        /// <summary>
        /// 获取默认数据库
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DBComponent GetDB(this DBManagerComponent self)
        {
            DBComponent db = self.CommonDB;
            if (db)
            {
                return db;
            }

            StartZoneConfig startZoneConfig = StartZoneConfigCategory.Instance.Get(0);
            if (startZoneConfig.DBConnection == "")
            {
                throw new Exception($"common db not found mongo connect string");
            }

            db = self.AddChildWithId<DBComponent, string, string>(0, startZoneConfig.DBConnection, startZoneConfig.DBName);
            self.CommonDB = db;
            return db;
        }
    }
}