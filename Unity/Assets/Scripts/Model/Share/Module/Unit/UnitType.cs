namespace ET
{
    public enum UnitType: byte
    {
        /// <summary>
        /// 玩家
        /// </summary>
        Player = 1,

        /// <summary>
        /// 怪物
        /// </summary>
        Monster = 2,
        
        /// <summary>
        /// 宠物
        /// </summary>
        Pet = 3,

        /// <summary>
        /// 采集物
        /// </summary>
        Collect = 4,

        /// <summary>
        /// 掉落物
        /// </summary>
        Drop = 5,
        
        /// <summary>
        /// 坐骑
        /// </summary>
        Mount = 6,

        /// <summary>
        /// 召唤物
        /// </summary>
        Summon = 7,

        Npc = 8,

        /// <summary>
        /// 环境生物
        /// </summary>
        Env = 10,
    }

    /// <summary>
    /// 角色子类型定义
    /// </summary>
    public enum RoleSubType
    {
        None = 0,

        Boss = 21,

        /// <summary>
        /// 精英
        /// </summary>
        Elite = 22,

        /// <summary>
        /// 小怪
        /// </summary>
        Monster = 23,

        /// <summary>
        /// 环境生物-箱子 
        /// </summary>
        EnvBox = 100,

        /// <summary>
        /// 环境生物-小精灵
        /// </summary>
        EnvElf = 101,
    }
}