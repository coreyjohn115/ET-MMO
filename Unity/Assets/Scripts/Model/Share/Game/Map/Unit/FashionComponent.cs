using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public struct UpdateFashionEffectEvent
    {
        public long RoleId;
        public FashionEffectType Key;
        public long Value;
    }

    [ComponentOf(typeof (Unit))]
    public class FashionComponent: Entity, IAwake, IDestroy, ITransfer
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<FashionEffectType, long> FashionEffects { get; set; }
    }
}