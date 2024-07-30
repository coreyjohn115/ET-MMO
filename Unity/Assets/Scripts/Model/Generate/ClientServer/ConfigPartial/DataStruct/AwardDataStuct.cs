using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 奖励类型
    /// </summary>
    public enum AwardType
    {
        /// <summary>
        /// 掉落ID
        /// </summary>
        Drop = 1,

        /// <summary>
        /// 道具ID+数量
        /// </summary>
        Item = 2,
    }

    /// <summary>
    /// 通用奖励数据结构
    /// </summary>
    public struct AwardDataStuct
    {
        public int MinLevel;
        public int MaxLevel;
        public AwardType AwardType;
        public List<long> Args;

        public AwardDataStuct(int minLevel, int maxLevel, AwardType awardType, List<long> args)
        {
            MinLevel = minLevel;
            MaxLevel = maxLevel;
            AwardType = awardType;
            Args = args;
        }
    }
}