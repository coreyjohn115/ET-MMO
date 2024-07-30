namespace ET.Client
{
    [EntitySystemOf(typeof (ClientChatUnit))]
    public static partial class ClientChatUnitSystem
    {
        [EntitySystem]
        private static void Awake(this ClientChatUnit self)
        {
        }

        public static void FromProto(this ClientChatUnit self, ChatMsgProto proto)
        {
            self.Time = proto.Time;
            self.Channel = (ChatChannelType)proto.Channel;
            self.Message = proto.Message;
            self.GroupId = proto.GroupId;
            self.RoleInfo = proto.RoleInfo;
            self.Data = ChatHelper.Decode(self.Message);
        }
    }
}