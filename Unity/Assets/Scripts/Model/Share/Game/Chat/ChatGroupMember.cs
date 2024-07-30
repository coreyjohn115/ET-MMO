namespace ET
{
    [ChildOf(typeof (ChatGroup))]
    public class ChatGroupMember: Entity, IAwake, ISerializeToEntity
    {
        public long sort;

        /// <summary>
        /// 消息免打扰
        /// </summary>
        public bool noDisturbing;

        public HeadProto headIcon;
    }
}