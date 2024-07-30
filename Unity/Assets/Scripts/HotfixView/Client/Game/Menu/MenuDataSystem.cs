namespace ET.Client
{
    [EntitySystemOf(typeof (MeunData))]
    [FriendOf(typeof (MeunData))]
    public static partial class MenuDataSystem
    {
        [EntitySystem]
        private static void Awake(this MeunData self, int id)
        {
            self.cfgId = id;
        }
    }
}