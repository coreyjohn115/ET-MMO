namespace ET.Server
{
    /// <summary>
    /// 玩家额外数据
    /// </summary>
    [UnitCom]
    [ComponentOf(typeof (Unit))]
    public class UnitExtra: Entity, IAwake, ITransfer, ICache
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public long LoginTime { get; set; }

        /// <summary>
        /// 下线时间
        /// </summary>
        public long LogoutTime { get; set; }

        /// <summary>
        /// 总在线时间
        /// </summary>
        public long TotalOnlineTime { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginCount { get; set; }

        /// <summary>
        /// 上次零点时间
        /// </summary>
        public long LastZeroTime { get; set; }

        public int Channel { get; set; }

        /// <summary>
        /// 上次周零点时间
        /// </summary>
        public long LastWeekTime { get; set; }

        /// <summary>
        /// 上次月零点时间
        /// </summary>
        public long LastMonthTime { get; set; }
    }
}