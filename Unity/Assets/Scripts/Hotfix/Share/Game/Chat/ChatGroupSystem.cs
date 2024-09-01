using System.Collections.Generic;

namespace ET
{
    [EntitySystemOf(typeof (ChatGroup))]
    [FriendOf(typeof (ChatGroup))]
    [FriendOf(typeof (ChatGroupMember))]
    public static partial class ChatGroupSystem
    {
        [EntitySystem]
        private static void Awake(this ChatGroup self)
        {
        }

        public static List<long> RoleList(this ChatGroup self)
        {
            List<long> list = new();
            foreach (long id in self.Children.Keys)
            {
                list.Add(id);
            }

            return list;
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