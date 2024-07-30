namespace ET.Server
{
    [EntitySystemOf(typeof (ChatUnit))]
    [FriendOf(typeof (ChatUnit))]
    public static partial class ChatUnitSystem
    {
        [EntitySystem]
        private static void Awake(this ChatUnit self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ChatUnit self)
        {
        }

        public static void UpdateInfo(this ChatUnit self, PlayerInfoProto playerInfo)
        {
            self.name = playerInfo.Name;
            self.headIcon = playerInfo.HeadIcon;
            self.level = playerInfo.Level;
            self.fight = playerInfo.Fight;
            self.sex = playerInfo.Sex;
        }

        public static PlayerInfoProto ToPlayerInfo(this ChatUnit self)
        {
            var playerInfo = PlayerInfoProto.Create();
            playerInfo.Id = self.Id;
            playerInfo.Name = self.name;
            playerInfo.HeadIcon = self.headIcon;
            playerInfo.Level = self.level;
            playerInfo.Fight = self.fight;
            playerInfo.Sex = self.sex;
            return playerInfo;
        }
    }
}