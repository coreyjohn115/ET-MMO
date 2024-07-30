using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server
{
    /// <summary>
    /// 缓存组件
    /// </summary>
    [ComponentOf(typeof (Scene))]
    public class CacheComponent: Entity, IAwake, IDestroy, ILoad
    {
        public Dictionary<string, UnitCache> cacheDict = new(10);

        [BsonIgnore]
        public List<string> CacheKeyList => cacheKeyList;

        public List<string> cacheKeyList = new ListComponent<string>();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, List<Entity>> needSaveDict = new(100);

        public long checkExpireTimer;
    }
}