using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server;

[ChildOf(typeof (MapManagerComponent))]
public class MapUnit: Entity, IAwake<int>, IDestroy, ISerializeToEntity
{
    /// <summary>
    /// 地图配置ID
    /// </summary>
    [BsonIgnore]
    public int MapId => this.mapId;

    public int mapId;

    public MapConfig MapConfig => MapConfigCategory.Instance.Get(this.MapId);

    public int fiberId;
    public ActorId actorId;
    public string actorStr;
    public CreateMapCtx ctx;

    /// <summary>
    /// 当前地图人数
    /// </summary>
    public int count;

    public long closeTime;
    public long validTime;
}