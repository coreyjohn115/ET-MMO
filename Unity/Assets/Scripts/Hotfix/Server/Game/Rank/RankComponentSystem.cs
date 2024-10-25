using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ET.Server
{
    [FriendOf(typeof (RankComponent))]
    [EntitySystemOf(typeof (RankComponent))]
    public static partial class RankComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RankComponent self)
        {
            self.rankDict.Clear();
            self.rankObjDict.Clear();
            self.rankItem.Clear();
            self.needSaveObj.Clear();
            self.Init();
            self.LoadRank().NoContext();
            self.Timer = self.Fiber().Root.GetComponent<TimerComponent>().NewRepeatedTimer(10 * 1000, TimerInvokeType.SaveRank, self);
        }

        [EntitySystem]
        private static void Destroy(this RankComponent self)
        {
            self.Fiber().Root.GetComponent<TimerComponent>().Remove(ref self.Timer);
            self.rankDict.Clear();
            self.rankObjDict.Clear();
            foreach ((string _, RankItemComponent com) in self.rankItem)
            {
                com.NeedSaveInfo.Clear();
            }

            self.rankItem.Clear();
            self.needSaveObj.Clear();
        }

        [EntitySystem]
        private static void Load(this RankComponent self)
        {
            self.Init();
        }

        [Invoke(TimerInvokeType.SaveRank)]
        public class SaveRankTimer: ATimer<RankComponent>
        {
            protected override void Run(RankComponent self)
            {
                if (self.IsClearData())
                {
                    return;
                }

                self.Save().NoContext();
            }
        }

        private static void Init(this RankComponent self)
        {
            self.loadRankDict = new Dictionary<int, RankItemConfig>()
            {
                { RankType.Fight, new RankItemConfig() { SubTypes = [0], IsIncrease = false } },
                { RankType.Level, new RankItemConfig() { SubTypes = [0], IsIncrease = false } },
            };
        }

        /// <summary>
        /// 加载子排行榜数据
        /// </summary>
        /// <param name="self"></param>
        /// <param name="zoneDb"></param>
        /// <param name="rankName"></param>
        /// <param name="subT"></param>
        private static async ETTask LoadSubRank(this RankComponent self, DBComponent zoneDb, string rankName, int subT)
        {
            var rankList = await zoneDb.Query<RankInfo>(info => true, rankName);
            if (!self.rankDict.TryGetValue(rankName, out var list))
            {
                list = new SortedList<RankInfo, long>(1000, self.RankComparer);
                self.rankDict.Add(rankName, list);
            }

            if (!self.rankItem.TryGetValue(rankName, out var item))
            {
                item = self.AddChild<RankItemComponent, int>(subT);
                self.rankItem.Add(rankName, item);
            }

            foreach (RankInfo info in rankList)
            {
                item.AddChild(info);
                list.Add(info, info.Id);
            }
        }

        /// <summary>
        /// 加载排行榜数据
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        private static async ETTask LoadRank(this RankComponent self)
        {
            DBComponent zoneDb = self.Scene().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());
            foreach ((int t, RankItemConfig config) in self.loadRankDict)
            {
                foreach (int sub in config.SubTypes)
                {
                    string rankName = GetRankName(t, sub, self.Zone());
                    await self.LoadSubRank(zoneDb, rankName, sub);
                }
            }

            Log.Info("加载排行榜数据完成!");
        }

        /// <summary>
        /// 保存排行榜数据
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask Save(this RankComponent self)
        {
            var zoneDb = self.Scene().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());
            if (self.needSaveObj.Count > 0)
            {
                using var list = ListComponent<ETTask>.Create();
                foreach (long l in self.needSaveObj)
                {
                    if (self.rankObjDict.TryGetValue(l, out RankObject obj))
                    {
                        list.Add(zoneDb.Save(obj));
                    }
                }

                self.needSaveObj.Clear();
                await ETTaskHelper.WaitAll(list);
            }

            foreach ((_, RankItemComponent item) in self.rankItem)
            {
                if (item.NeedSaveInfo.Count <= 0)
                {
                    continue;
                }

                using var list = ListComponent<ETTask>.Create();
                foreach (long l in item.NeedSaveInfo)
                {
                    var child = item.GetChild<RankInfo>(l);
                    if (child != null)
                    {
                        list.Add(zoneDb.Save(child, GetRankName(child.RankType, child.SubType, child.Zone)));
                    }
                }

                item.NeedSaveInfo.Clear();
                await ETTaskHelper.WaitAll(list);
            }
        }

        public static void UpdateRankObj(this RankComponent self, long unitId, RankObject obj)
        {
            if (obj == default)
            {
                return;
            }

            if (!self.rankObjDict.TryGetValue(unitId, out RankObject info))
            {
                self.rankObjDict.Add(unitId, obj);
            }
            else
            {
                info.Dispose();
                self.rankObjDict[unitId] = obj;
            }

            self.needSaveObj.Add(unitId);
        }

        /// <summary>
        /// 更新排行榜
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unitId"></param>
        /// <param name="t">主榜类型</param>
        /// <param name="subT">子榜类型</param>
        /// <param name="time">更新时间</param>
        /// <param name="info"></param>
        /// <param name="score">分数</param>
        /// <returns></returns>
        public static void UpdateRank(this RankComponent self, long unitId, int t, int subT, long score, RankObject info = null,
        long? time = null)
        {
            self.UpdateRankObj(unitId, info);
            string rankName = GetRankName(t, subT, self.Zone());
            if (!self.rankDict.TryGetValue(rankName, out var list))
            {
                list = new SortedList<RankInfo, long>(self.RankComparer);
                self.rankDict.Add(rankName, list);
            }

            if (!self.rankItem.TryGetValue(rankName, out RankItemComponent item))
            {
                item = self.AddChild<RankItemComponent, int>(subT);
                self.rankItem.Add(rankName, item);
            }

            RankInfo oldInfo = null;
            int index = list.IndexOfValue(unitId);
            if (index >= 0)
            {
                oldInfo = list.Keys[index];
                list.RemoveAt(index);
                item.RemoveChild(unitId);
            }

            bool isIncrease = self.loadRankDict[t].IsIncrease;
            RankInfo child = item.AddChildWithId<RankInfo>(unitId);
            if (isIncrease)
            {
                child.Score = oldInfo? oldInfo.Score + score : score;
            }
            else
            {
                child.Score = score;
            }

            child.Time = oldInfo? oldInfo.Time : time ?? TimeInfo.Instance.FrameTime;
            child.RankType = t;
            child.SubType = subT;
            child.Zone = self.Zone();
            list.Add(child, unitId);
            item.NeedSaveInfo.Add(unitId);

            oldInfo?.Dispose();
        }

        public static (int, long) GetRankScore(this RankComponent self, long unitId, int t, int subT)
        {
            var rankName = GetRankName(t, subT, self.Zone());
            if (!self.rankDict.TryGetValue(rankName, out var sortList))
            {
                return (-1, 0L);
            }

            int index = sortList.IndexOfValue(unitId);
            if (index >= 0)
            {
                return (index + 1, sortList.Keys[index].Score);
            }

            return (-1, 0L);
        }

        public static async ETTask<(List<RankInfoProto>, RankInfoProto)> GetRank(this RankComponent self, long unitId, int t, int subT,
        int page = 0)
        {
            var list = new List<RankInfoProto>();
            var rankName = GetRankName(t, subT, self.Zone());
            if (!self.rankDict.TryGetValue(rankName, out var sortList))
            {
                var zoneDb = self.Scene().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());
                await self.LoadSubRank(zoneDb, rankName, subT);
                sortList = self.rankDict[rankName];
            }

            RankInfoProto selfRank = default;
            for (int i = page * ConstValue.RankPage; i < Math.Min(sortList.Count, (page + 1) * ConstValue.RankPage); i++)
            {
                var info = sortList.Keys[i];
                list.Add(self.CreateProto(info, i + 1));
            }

            int index = sortList.IndexOfValue(unitId);
            if (index >= 0)
            {
                selfRank = self.CreateProto(sortList.Keys[index], index + 1);
            }

            return (list, selfRank);
        }

        /// <summary>
        /// 清除指定排行榜下的全部子榜
        /// </summary>
        /// <param name="self"></param>
        /// <param name="t"></param>
        public static async ETTask ClearRank(this RankComponent self, int t)
        {
            var zoneDb = self.Scene().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());
            var options = new ListCollectionNamesOptions
            {
                Filter = Builders<BsonDocument>.Filter.Regex("name", new BsonRegularExpression($"^Rank_{t}_{self.Zone()}_"))
            };
            var collections = await zoneDb.GetCollections(self.GetHashCode(), options);
            foreach (string rankName in collections)
            {
                if (!self.rankDict.Remove(rankName, out var sortList))
                {
                    await zoneDb.RemoveCollection<RankInfo>(self.GetHashCode(), rankName);
                    return;
                }

                foreach (RankInfo info in sortList.Keys)
                {
                    info.Dispose();
                }

                sortList.Clear();
                await zoneDb.RemoveCollection<RankInfo>(self.GetHashCode(), rankName);
            }
        }

        /// <summary>
        /// 清除指定排行榜下的指定子榜
        /// </summary>
        /// <param name="self"></param>
        /// <param name="t"></param>
        /// <param name="subT"></param>
        public static async ETTask ClearRank(this RankComponent self, int t, int subT)
        {
            var rankName = GetRankName(t, subT, self.Zone());
            var zoneDb = self.Scene().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());
            if (!self.rankDict.Remove(rankName, out var sortList))
            {
                await zoneDb.RemoveCollection<RankInfo>(self.GetHashCode(), rankName);
                return;
            }

            foreach (RankInfo info in sortList.Keys)
            {
                info.Dispose();
            }

            sortList.Clear();
            await zoneDb.RemoveCollection<RankInfo>(self.GetHashCode(), rankName);
        }

        private static RankInfoProto CreateProto(this RankComponent self, RankInfo info, long rank)
        {
            RankInfoProto proto = RankInfoProto.Create();
            proto.Id = info.Id;
            proto.Score = info.Score;
            proto.Rank = rank;
            proto.Time = info.Time;

            if (!self.rankObjDict.TryGetValue(proto.Id, out RankObject obj))
            {
                return proto;
            }

            RankRoleInfoProto roleInfoProto = RankRoleInfoProto.Create();
            roleInfoProto.Name = obj.RoleObj.Name;
            roleInfoProto.HeadIcon = obj.RoleObj.HeadIcon;
            roleInfoProto.Level = obj.RoleObj.Level;
            roleInfoProto.Fight = obj.RoleObj.Fight;
            roleInfoProto.Sex = obj.RoleObj.Sex;

            return proto;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetRankName(int t, int subT, int zone)
        {
            return $"Rank_{t}_{zone}_{subT}";
        }
    }
}