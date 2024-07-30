namespace ET
{
    /// <summary>
    /// 排行榜类型
    /// </summary>
    public enum RankType
    {
        /// <summary>
        /// 等级排行
        /// </summary>
        Level = 1,

        /// <summary>
        /// 战斗力排行
        /// </summary>
        Fight,
    }

    public class RankSubType
    {
        /// <summary>
        /// 主榜
        /// </summary>
        public const int Master = 0;

        /// <summary>
        /// 日榜
        /// </summary>
        public const int Day = 1;

        /// <summary>
        /// 周榜
        /// </summary>
        public const int Week = 2;

        /// <summary>
        /// 月榜
        /// </summary>
        public const int Month = 3;
    }
}