namespace ET.Server;

[EntitySystemOf(typeof (MapUnit))]
[FriendOf(typeof (MapUnit))]
public static partial class MapUnitSystem
{
    [EntitySystem]
    private static void Awake(this MapUnit self, int mapId)
    {
        self.mapId = mapId;
    }

    [EntitySystem]
    private static void Destroy(this MapUnit self)
    {
        self.closeTime = 0;
        self.validTime = 0;
        self.fiberId = 0;
        self.mapId = 0;
    }

    public static void AddCount(this MapUnit self)
    {
        self.count++;
    }

    public static void RemoveCount(this MapUnit self)
    {
        self.count--;
    }

    /// <summary>
    /// 当前地图是否可用
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static bool IsAvailable(this MapUnit self)
    {
        if (self.validTime > 0 || (self.closeTime > 0 && TimeInfo.Instance.Frame >= self.closeTime))
        {
            return false;
        }

        if (self.count >= self.MapConfig.MaxPlayer)
        {
            return false;
        }

        return true;
    }
}