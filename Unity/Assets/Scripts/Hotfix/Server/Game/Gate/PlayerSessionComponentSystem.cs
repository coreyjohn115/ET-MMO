namespace ET.Server
{
    [EntitySystemOf(typeof (PlayerSessionComponent))]
    public static partial class PlayerSessionComponentSystem
    {
        [EntitySystem]
        private static void Destroy(this PlayerSessionComponent self)
        {
            Scene root = self.Root();
            if (root.IsDisposed)
            {
                return;
            }

            self.RemoveLocation(LocationType.GateSession).NoContext();
        }

        [EntitySystem]
        private static void Awake(this PlayerSessionComponent self)
        {
        }
    }
}