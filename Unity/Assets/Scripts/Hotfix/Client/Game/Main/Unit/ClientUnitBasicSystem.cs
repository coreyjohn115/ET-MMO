namespace ET.Client
{
    [EntitySystemOf(typeof (UnitBasic))]
    [FriendOf(typeof (UnitBasic))]
    public static partial class ClientUnitBasicSystem
    {
        [EntitySystem]
        private static void Awake(this UnitBasic self)
        {
        }

        public static void UpdateBasicInfo(this UnitBasic self, UnitBasicProto proto)
        {
            self.Gid = proto.Gid;
            self.UserUid = proto.UserUid;
            self.PlayerName = proto.PlayerName;
            self.level = proto.Level;
            self.HeadIcon.FromProto(proto.HeadIcon);
            self.Sex = proto.Sex;
            self.VipLevel = proto.VipLevel;
            self.TotalFight = proto.TotalFight;

            EventSystem.Instance.Publish(self.Scene(), new BasicChangeEvent() { Unit = self.GetParent<Unit>() });
        }
    }
}