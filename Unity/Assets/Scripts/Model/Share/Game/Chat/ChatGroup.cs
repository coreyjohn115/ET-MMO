namespace ET
{
    [ChildOf]
    public class ChatGroup: Entity, IAwake
    {
        public string name;
        public long leaderId;
        public ChatChannelType channel;
    }
}