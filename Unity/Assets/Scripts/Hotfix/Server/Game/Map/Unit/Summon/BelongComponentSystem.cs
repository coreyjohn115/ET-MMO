namespace ET.Server;

[EntitySystemOf(typeof (BelongComponent))]
public static partial class BelongComponentSystem
{
    [EntitySystem]
    private static void Awake(this BelongComponent self)
    {
    }

    public static void SetBelong(this BelongComponent self, long belong)
    {
        self.belongId = belong;
    }
}