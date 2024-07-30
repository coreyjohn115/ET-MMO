namespace ET.Client
{
    public enum LoopType
    {
        None,
        /// <summary>
        /// 每次播放完成 重新开始
        /// </summary>
        Restart,

        /// <summary>
        /// 每次播放完成 反向播放
        /// </summary>
        PingPong,
    }
}