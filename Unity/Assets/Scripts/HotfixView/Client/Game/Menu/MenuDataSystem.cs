namespace ET.Client
{
    [EntitySystemOf(typeof (MenuData))]
    [FriendOf(typeof (MenuData))]
    public static partial class MenuDataSystem
    {
        [EntitySystem]
        private static void Awake(this MenuData self, int id)
        {
            self.cfgId = id;
        }
    }
}