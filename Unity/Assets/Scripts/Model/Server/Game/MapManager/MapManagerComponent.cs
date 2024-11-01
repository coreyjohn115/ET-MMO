using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server;

[ComponentOf(typeof (Scene))]
public class MapManagerComponent: Entity, IAwake, IDestroy
{
    public long timer;

    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<int, HashSet<long>> mapCfgDict = [];

    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<long, long> roleMapDict = [];
}