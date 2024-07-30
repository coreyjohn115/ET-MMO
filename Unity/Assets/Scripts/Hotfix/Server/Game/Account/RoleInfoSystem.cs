namespace ET.Server
{
    [FriendOf(typeof (RoleInfo))]
    public static class RoleInfoSystem
    {
        public static void FromMessage(this RoleInfo self, RoleInfoProto roleInfoProto)
        {
            self.Name = roleInfoProto.Name;
            self.State = roleInfoProto.State;
            self.Account = roleInfoProto.Account;
            self.CreateTime = roleInfoProto.CreateTime;
            self.ServerId = roleInfoProto.ServerId;
            self.LastLoginTime = roleInfoProto.LastLoginTime;
        }

        public static RoleInfoProto ToMessage(this RoleInfo self)
        {
            RoleInfoProto proto = RoleInfoProto.Create();
            proto.Id = self.Id;
            proto.Name = self.Name;
            proto.State = self.State;
            proto.Account = self.Account;
            proto.CreateTime = self.CreateTime;
            proto.ServerId = self.ServerId;
            proto.LastLoginTime = self.LastLoginTime;
            return proto;
        }
    }
}