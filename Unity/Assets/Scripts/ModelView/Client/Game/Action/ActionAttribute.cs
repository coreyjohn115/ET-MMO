namespace ET.Client
{
    public class ActionAttribute: BaseAttribute
    {
        public string ActionType { get; }

        public ActionAttribute(string name)
        {
            this.ActionType = name;
        }
    }

    public abstract class AAction
    {
        public virtual void Execute(Unit unit, ActionSubUnit actionUnit)
        {
        }

        /// <summary>
        /// 行为开始执行
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="actionUnit"></param>
        public virtual async ETTask OnExecute(Unit unit, ActionSubUnit actionUnit)
        {
            await ETTask.CompletedTask;
        }

        /// <summary>
        /// 每帧调用
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="actionUnit"></param>
        public virtual void OnUpdate(Unit unit, ActionSubUnit actionUnit)
        {
        }

        /// <summary>
        /// 行为退出执行
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="actionUnit"></param>
        public virtual void OnUnExecute(Unit unit, ActionSubUnit actionUnit)
        {
        }

        /// <summary>
        /// 行为已完成
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="actionUnit"></param>
        public virtual void OnFinish(Unit unit, ActionSubUnit actionUnit)
        {
        }
    }
}