using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server
{
    [UnitCom]
    [ComponentOf(typeof (Unit))]
    public class ShieldComponent: Entity, IAwake, ITransfer
    {
        /// <summary>
        /// 护盾字典
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> shieldIdDict = [];
    }
}