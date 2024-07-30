using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server
{
    [UnitCom]
    [ComponentOf(typeof (Unit))]
    public class BuffComponent: Entity, IAwake, IUpdate, ICache, ITransfer
    {
        [BsonIgnore]
        public Dictionary<int, int> BuffRateTime => this.buffRateTime;

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, int> buffRateTime = [];

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, int> scribeEventMap = [];

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, bool> eventMap = [];
    }
}