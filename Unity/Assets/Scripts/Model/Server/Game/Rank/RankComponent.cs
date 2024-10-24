using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server
{
    public class RankCompare: IComparer<RankInfo>
    {
        public int Compare(RankInfo x, RankInfo y)
        {
            if (x.Score != y.Score)
            {
                return y.Score.CompareTo(x.Score);
            }

            return x.Time.CompareTo(y.Time);
        }
    }

    public class RankItemConfig
    {
        public List<int> SubTypes;

        /// <summary>
        ///  是否累加
        /// </summary>
        public bool IsIncrease;
    }

    [ComponentOf(typeof (Scene))]
    public class RankComponent: Entity, IAwake, IDestroy, ILoad
    {
        public long Timer = 0L;

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, RankItemConfig> loadRankDict = new();

        [BsonIgnore]
        private IComparer<RankInfo> rankComparer;

        /// <summary>
        /// 排行榜排序方法
        /// </summary>
        [BsonIgnore]
        public IComparer<RankInfo> RankComparer
        {
            get
            {
                if (rankComparer != null)
                {
                    return rankComparer;
                }

                rankComparer = new RankCompare();
                return this.rankComparer;
            }
        }

        public HashSet<long> needSaveObj = new(100);

        /// <summary>
        /// 角色数据
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, RankObject> rankObjDict = new();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<string, RankItemComponent> rankItem = new();

        /// <summary>
        /// 排行榜名称对应排行数据字典
        /// </summary>
        [BsonIgnore]
        public Dictionary<string, SortedList<RankInfo, long>> rankDict = new(100);
    }
}