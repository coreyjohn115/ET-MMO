namespace ET.Server;

[EntitySystemOf(typeof (MapComponent))]
[FriendOf(typeof (MapComponent))]
public static partial class MapComponentSystem
{
    [EntitySystem]
    private static void Awake(this MapComponent self)
    {
    }

    public static async ETTask InitMap(this MapComponent self, CreateMapCtx ctx)
    {
        self.ctx = ctx;
        await ETTask.CompletedTask;
    }
}