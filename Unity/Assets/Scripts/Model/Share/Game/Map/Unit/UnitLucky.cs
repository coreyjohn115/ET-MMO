using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    /// <summary>
    /// 玩家额外数据
    /// </summary>
    [UnitCom]
    [ComponentOf(typeof (Unit))]
    public class UnitLucky: Entity, IAwake, ITransfer, ICache
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> banDict = new();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> recoverDict = new();

        /// <summary>
        /// 战斗力列表
        /// </summary>
        public List<long> fightList = new((int)FightType.Max);

        public List<long> flexData = new();
    }
}