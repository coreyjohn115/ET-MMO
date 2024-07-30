namespace ET
{
    /// <summary>
    /// 元素类型
    /// </summary>
    public enum ElementType
    {
        None = 0,
        
        /// <summary>
        /// 水元素
        /// </summary>
        Water = 1,

        /// <summary>
        /// 火元素
        /// </summary>
        Fire = 2,

        /// <summary>
        /// 雷元素
        /// </summary>
        Thunder = 3,

        /// <summary>
        /// 冰元素
        /// </summary>
        Ice = 4,
        
        /// <summary>
        /// 风元素
        /// </summary>
        Wind = 5,
        
        /// <summary>
        /// 草元素
        /// </summary>
        Grass = 6,
    }

    /// <summary>
    /// 目标类型
    /// </summary>
    public enum FocusType
    {
        /// <summary>
        /// 任意
        /// </summary>
        All = 0,

        /// <summary>
        /// 自身
        /// </summary>
        Self = 1,

        /// <summary>
        /// 敌对 
        /// </summary>
        OOP = 2,

        /// <summary>
        /// 友善
        /// </summary>
        Friend = 3,

        /// <summary>
        /// 友善排己
        /// </summary>
        FriendNSelf = 4,

        /// <summary>
        /// 队友
        /// </summary>
        Team = 5,

        /// <summary>
        /// 队友排己
        /// </summary>
        TeamNSelf = 6,
    }

    /// <summary>
    /// 范围类型
    /// </summary>
    public enum RangeType
    {
        None = 0,
        
        /// <summary>
        /// 自身
        /// </summary>
        Self = 1,

        /// <summary>
        /// 单一目标
        /// </summary>
        Single = 2,
        
        Much = 3,
        
        /// <summary>
        /// 自身扇形
        /// </summary>
        SelfFan = 4,

        /// <summary>
        /// 自身矩形
        /// </summary>
        SelfLine = 5,

        /// <summary>
        /// 目标矩形
        /// </summary>
        DstLine = 6,

        /// <summary>
        /// 目标扇形
        /// </summary>
        DstFan = 7,

        /// <summary>
        /// 目标扇形+直线
        /// </summary>
        DstFanLine = 8,
        
        /// <summary>
        /// 自身扇形+直线
        /// </summary>
        SelfFanLine = 9,

        /// <summary>
        /// 使用上次(技能)
        /// </summary>
        UseLast = 11,
    }

    /// <summary>
    /// 角色阵营定义
    /// </summary>
    public enum CampType
    {
        Npc = 0,

        /// <summary>
        /// 怪物阵营
        /// </summary>
        Monster = 1,

        /// <summary>
        /// 玩家阵营
        /// </summary>
        Player = 2,
    }

    /// <summary>
    /// 角色能力码
    /// </summary>
    public enum RoleAbility
    {
        /// <summary>
        /// 普通攻击
        /// </summary>
        Attack = 1,

        /// <summary>
        /// 免疫
        /// </summary>
        ImmUnity = 2,

        /// <summary>
        /// 使用道具
        /// </summary>
        UseItem = 3,

        /// <summary>
        /// 隐身
        /// </summary>
        Hide = 4,

        /// <summary>
        /// 使用技能
        /// </summary>
        Skill = 5,

        /// <summary>
        /// 无敌
        /// </summary>
        Invincible = 6,

        /// <summary>
        /// 移动
        /// </summary>
        Move = 7,

        End = 32,
    }
}