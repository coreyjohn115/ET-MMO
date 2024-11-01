namespace ET.Server
{
    [EntitySystemOf(typeof (UnitBasic))]
    [FriendOf(typeof (UnitBasic))]
    public static partial class UnitBasicSystem
    {
        [EntitySystem]
        private static void Awake(this UnitBasic self)
        {
            if (self.level == 0)
            {
                self.level = 1;
            }
        }

        public static void Initialize(this UnitBasic self, Player Player)
        {
            
        }

        public static PlayerInfoProto ToPlayerInfo(this UnitBasic self)
        {
            PlayerInfoProto info = PlayerInfoProto.Create();
            info.Id = self.Id;
            info.Name = self.PlayerName;
            info.HeadIcon = self.HeadIcon.ToProto();
            info.Level = self.Level;
            info.Fight = self.TotalFight;
            info.Sex = self.Sex;
            return info;
        }

        public static UnitBasicProto GetBasicProto(this UnitBasic self)
        {
            UnitBasicProto pkg = UnitBasicProto.Create();
            pkg.Gid = self.Gid;
            pkg.UserUid = self.UserUid;
            pkg.PlayerName = self.PlayerName;
            pkg.Level = self.Level;
            pkg.VipLevel = self.VipLevel;
            pkg.HeadIcon = self.HeadIcon.ToProto();
            pkg.Sex = self.Sex;
            pkg.TotalFight = self.TotalFight;
            return pkg;
        }
    }
}