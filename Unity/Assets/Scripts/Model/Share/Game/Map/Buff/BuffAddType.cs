namespace ET
{
    /// <summary>
    /// Buff叠加方式
    /// </summary>
    public enum BuffAddType
    {
        /// <summary>
        /// 增加时长
        /// </summary>
        AddTime = 1,
        
        /// <summary>
        /// 独立新增
        /// </summary>
        New = 2,
        
        /// <summary>
        /// 替换
        /// </summary>
        Replace = 3,
        
        /// <summary>
        /// 重置持续时长
        /// </summary>
        ResetTime = 4,
        
        /// <summary>
        /// 自身互斥
        /// </summary>
        SelfMutex = 5,
        
        /// <summary>
        /// 角色独立
        /// </summary>
        Role = 6,
        
        /// <summary>
        /// 类型互斥
        /// </summary>
        ClassifyMutex = 7,
    }
}