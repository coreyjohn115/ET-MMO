namespace ET.Server
{
    public enum ActivityStatus
    {
        /// <summary>
        /// 关闭中
        /// </summary>
        Close = 0,

        /// <summary>
        /// 显示中
        /// </summary>
        Show,

        /// <summary>
        /// 开启中
        /// </summary>
        Open,

        /// <summary>
        /// 关闭显示
        /// </summary>
        RealClose,
    }

    [ChildOf(typeof (ActivityComponent))]
    public class ActivityData: Entity, IAwake, IDestroy
    {
        public string Key;
        public ActivityType Type;
        public long RoleId;
        public ActivityStatus Status;
        public long ShowTime;
        public long OpenTime;
        public long CloseTime;
        public long RealCloseTime;
    }
}