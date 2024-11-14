namespace ET.Server
{
    public enum BuffLife
    {
        /// <summary>
        /// Buff创建时
        /// </summary>
        OnCreate,

        /// <summary>
        /// 每帧更新
        /// </summary>
        OnUpdate,

        /// <summary>
        /// 触发Buff实践
        /// </summary>
        OnEvent,

        /// <summary>
        /// Buff时间到
        /// </summary>
        OnTimeOut,

        /// <summary>
        /// Buff移除时
        /// </summary>
        OnRemove,
    }
}