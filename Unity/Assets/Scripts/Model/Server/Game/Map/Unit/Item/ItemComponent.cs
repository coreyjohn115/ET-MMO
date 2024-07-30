using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server
{
    public enum ItemMode
    {
        /// <summary>
        /// 开宝箱
        /// </summary>
        OpenBox = 1,
    }

    public struct AddItemData
    {
        public int LogEvent { get; init; }

        /// <summary>
        /// 是否同步给客户端
        /// </summary>
        public bool NotUpdate { get; init; }
    }

    [UnitCom]
    [ComponentOf(typeof (Unit))]
    public class ItemComponent: Entity, IAwake, IDestroy, ICache, ITransfer
    {
        public HashSet<int> initItemIds = new();

        /// <summary>
        /// 配置Id对应道具的实例ID列表
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, HashSet<long>> cfgIdDict = new();

        /// <summary>
        /// 期限道具
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, long> validItemDict = new();
    }
}