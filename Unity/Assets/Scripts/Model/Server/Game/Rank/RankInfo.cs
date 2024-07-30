namespace ET.Server
{
    [ChildOf(typeof (RankItemComponent))]
    public class RankInfo: Entity, IAwake
    {
        public long Score { get; set; }

        public long Time { get; set; }

        /// <summary>
        /// 排行榜类型
        /// </summary>
        public RankType RankType { get; set; }

        /// <summary>
        /// 子榜类型
        /// </summary>
        public int SubType { get; set; }

        public int Zone { get; set; }
    }
}