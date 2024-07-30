namespace ET
{
    [ChildOf(typeof(UnitLucky))]
    public class UnitItemRecover: Entity, IAwake
    {
        /// <summary>
        /// 下次恢复时间
        /// </summary>
        public long nextTime;

        /// <summary>
        /// 超过此数不再恢复
        /// </summary>
        public long maxCount;

        /// <summary>
        /// 本周期剩余可恢复数量
        /// </summary>
        public long leftCount;

        /// <summary>
        /// 已使用时间(对于在线时长恢复时 下线时会保存已用时)
        /// </summary>
        public long useTime;

        /// <summary>
        /// 本周期总共可恢复数量
        /// </summary>
        public long count;
    }
}