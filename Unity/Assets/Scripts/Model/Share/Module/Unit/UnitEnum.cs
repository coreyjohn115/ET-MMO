using Sirenix.OdinInspector;

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
        [LabelText("任意")]
        All = 0,

        /// <summary>
        /// 自身
        /// </summary>
        [LabelText("自身")]
        Self = 1,

        /// <summary>
        /// 敌对 
        /// </summary>
        [LabelText("敌对")]
        OOP = 2,

        /// <summary>
        /// 友善
        /// </summary>
        [LabelText("友善")]
        Friend = 3,

        /// <summary>
        /// 友善排己
        /// </summary>
        [LabelText("友善排己")]
        FriendNSelf = 4,

        /// <summary>
        /// 队友
        /// </summary>
        [LabelText("队友")]
        Team = 5,

        /// <summary>
        /// 队友排己
        /// </summary>
        [LabelText("队友排己")]
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
        [LabelText("自身")]
        Self = 1,

        /// <summary>
        /// 单一目标
        /// </summary>
        [LabelText("单一目标")]
        Single = 2,

        /// <summary>
        /// 自身扇形
        /// </summary>
        [LabelText("自身扇形")]
        SelfFan = 4,

        /// <summary>
        /// 自身矩形
        /// </summary>
        [LabelText("自身矩形")]
        SelfLine = 5,

        /// <summary>
        /// 目标矩形
        /// </summary>
        [LabelText("目标矩形")]
        DstLine = 6,

        /// <summary>
        /// 目标扇形
        /// </summary>
        [LabelText("目标扇形")]
        DstFan = 7,

        /// <summary>
        /// 目标扇形+直线
        /// </summary>
        [LabelText("目标扇形+直线")]
        DstFanLine = 8,

        /// <summary>
        /// 自身扇形+直线
        /// </summary>
        [LabelText("自身扇形+直线")]
        SelfFanLine = 9,

        /// <summary>
        /// 使用上次(技能)
        /// </summary>
        [LabelText("使用上次(技能)")]
        UseLast = 10,
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