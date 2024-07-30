namespace ET
{
    /// <summary>
    /// 任务事件类型
    /// </summary>
    public enum TaskEventType
    {
        /// <summary>
        /// 完成
        /// </summary>
        Complete = 1,

        /// <summary>
        /// 使用XX个XX道具
        /// </summary>
        UseCountItem = 2,
        
        /// <summary>
        /// 获得XX个X道具
        /// </summary>
        AddCountItem = 3,
        
        /// <summary>
        /// 获得XX个XX道具
        /// </summary>
        ConsumeCountItem = 4,
        
        /// <summary>
        /// 大本营升至XX级
        /// </summary>
        HomeLevel = 5,

        /// <summary>
        /// 联盟任任务事件类型
        /// </summary>
        LeagueType = 1000,

        /// <summary>
        /// 全服任务事件类型
        /// </summary>
        ServerType = 2000,
    }
}