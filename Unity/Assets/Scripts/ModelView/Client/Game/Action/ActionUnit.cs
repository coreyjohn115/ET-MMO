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

    [ChildOf(typeof (ActionComponent))]
    public class ActionUnit: Entity, IAwake<string, int>, IDestroy
    {
        public string ActionName { get; set; }

        /// <summary>
        /// 行为是否在执行中
        /// </summary>
        public bool IsRunning => this.state == ActionState.Run;

        /// <summary>
        /// 行为持续时间
        /// </summary>
        public float Interval => this.outDuration == 0? (float)this.Config.Duration : this.outDuration;

        public ActionConfig Config => ActionConfigCategory.Instance.GetActionCfg(this.ActionName);

        public AAction action;

        public ActionState state;

        //外部传入的时间
        public float outDuration;

        /// <summary>
        /// 行为当前执行时间
        /// </summary>
        public float duration;

        /// <summary>
        /// 行为开始时间
        /// </summary>
        public float startTime;
        
        public ETTask tcs;
    }
}