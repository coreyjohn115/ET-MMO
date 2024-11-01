namespace ET
{
    public enum MailBoxType
    {
        /// <summary>
        /// 消息是有序的, 处理完一个才能处理下一个
        /// </summary>
        OrderedMessage = 1,

        /// <summary>
        /// 消息是无序的, 可以同时处理多个消息
        /// </summary>
        UnOrderedMessage = 2,
        GateSession = 3,
    }
}