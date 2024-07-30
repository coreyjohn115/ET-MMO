namespace ET.Client
{
    public enum ChatMsgType
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        Time = 1,

        /// <summary>
        /// 系统消息
        /// </summary>
        System = 2,

        /// <summary>
        /// 文本
        /// </summary>
        Text = 3,

        /// <summary>
        /// 表情包
        /// </summary>
        Image = 4,

        /// <summary>
        /// 道具分享
        /// </summary>
        Item = 5,
    }
}