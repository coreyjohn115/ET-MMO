namespace ET
{
    /// <summary>
    /// Buff类型码
    /// </summary>
    public enum BuffClassify
    {
        /// <summary>
        /// 死亡删除
        /// </summary>
        Dead = 1,
        
        /// <summary>
        /// 使用技能后删除
        /// </summary>
        UseSkill = 2,
        
        /// <summary>
        /// 进入战斗时删除
        /// </summary>
        Fight = 4,
        
        /// <summary>
        /// 过图删除
        /// </summary>
        ChangeMap = 5,
        
        /// <summary>
        /// 复活后删除
        /// </summary>
        Relive = 7
    }
}