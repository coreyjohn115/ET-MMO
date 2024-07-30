namespace ET
{
    public enum BanType
    {
        /// <summary>
        /// 不限购
        /// </summary>
        None = 0,

        /// <summary>
        /// 购买后指定时间内限购
        /// </summary>
        BuySec = 1,

        /// <summary>
        /// 固定时间间隔限购（开服零点开始算)
        /// </summary>
        Time = 2,

        /// <summary>
        /// 每天限购
        /// </summary>
        Day = 3,

        /// <summary>
        /// 每周限购
        /// </summary>
        Week = 4,

        /// <summary>
        /// 每月限购
        /// </summary>
        Month = 5,

        /// <summary>
        /// 终生限购
        /// </summary>
        Life = 6,

        /// <summary>
        /// 活动限购
        /// </summary>
        Activity = 7,
    }

    [ChildOf(typeof(UnitLucky))]
    public class UnitItemBan: Entity, IAwake
    {
        /// <summary>
        /// 限购次数
        /// </summary>
        public long count;

        /// <summary>
        /// 限购类型
        /// </summary>
        public BanType canType;

        /// <summary>
        /// 限购有效期
        /// </summary>
        public long validTIme;

        /// <summary>
        /// 额外的限购次数
        /// </summary>
        public long extraBanCount;

        /// <summary>
        /// 活动id
        /// </summary>
        public int activityId;
    }
}