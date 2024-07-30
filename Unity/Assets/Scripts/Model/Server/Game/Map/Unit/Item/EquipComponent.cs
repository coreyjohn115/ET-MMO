using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server;

public struct EquipPutOnEvent
{
    public EntityRef<Unit> Unit;
    public EntityRef<ItemData> Item;
}

public struct EquipTakeOffEvent
{
    public EntityRef<Unit> Unit;
    public EntityRef<ItemData> Item;
}

[UnitCom]
[ComponentOf(typeof (Unit))]
public class EquipComponent: Entity, IAwake, ICache, ITransfer
{
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<EquipPosType, long> equipDict = new();
}