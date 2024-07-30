namespace ET
{
    [EntitySystemOf(typeof (ChatGroup))]
    [FriendOf(typeof (ChatGroup))]
    [FriendOf(typeof (ChatGroupMember))]
    public static partial class ChatGroupSystem
    {
        [EntitySystem]
        private static void Awake(this ChatGroup self, string guid)
        {
            self.guid = guid;
        }

        public static void FromProto(this ChatGroup self, ChatGroupProto proto)
        {
            self.name = proto.Name;
            self.leaderId = proto.LeaderId;
            foreach (var member in proto.MemberList)
            {
                ChatGroupMember m = self.AddChildWithId<ChatGroupMember>(member.RoleId);
                m.sort = member.Sort;
                m.headIcon = member.HeadIcon;
                m.noDisturbing = member.NoDisturbing;
            }
        }
    }
}