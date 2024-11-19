namespace ET.Client
{
    public enum ActionState
    {
        Ready,

        /// <summary>
        /// 行为运行中
        /// </summary>
        Run,

        /// <summary>
        /// 行为已完成
        /// </summary>
        Complete,

        /// <summary>
        /// 行为已结束
        /// </summary>
        Finish
    }

    [ChildOf(typeof (ActionUnit))]
    public class ActionSubUnit: Entity, IAwake<string, float, ActionConfig>, IDestroy
    {
        public string ActionName { get; set; }

        /// <summary>
        /// 行为是否在执行中
        /// </summary>
        public bool IsRunning => this.state == ActionState.Run;

        /// <summary>
        /// 行为持续时间
        /// </summary>
        public float Interval => this.outDuration == 0f? this.Config.Duration : this.outDuration;

        public ActionConfig Config { get; set; }

        public AAction action;

        public ActionState state;

        /// <summary>
        /// 外部传入的时间
        /// </summary>
        public float outDuration;

        /// <summary>
        /// 行为当前执行时间
        /// </summary>
        public float duration;

        /// <summary>
        /// 行为开始时间
        /// </summary>
        public float startTime;
    }
}