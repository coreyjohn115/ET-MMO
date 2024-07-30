namespace ET.Client
{
    [ChildOf(typeof (ClientChatComponent))]
    public class ClientChatUnit: Entity, IAwake
    {
        public long Time { get; set; }

        public PlayerInfoProto RoleInfo { get; set; }

        public ChatChannelType Channel { get; set; }

        public string Message { get; set; }

        public string GroupId { get; set; }

        public ChatMsgData Data { get; set; }
    }
}