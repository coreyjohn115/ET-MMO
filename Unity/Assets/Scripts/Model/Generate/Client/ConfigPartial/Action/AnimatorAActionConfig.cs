using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 动画行为配置
    /// </summary>
    public class AnimatorAActionConfig: ActionSubConfig
    {
        /// <summary>
        /// 动作对应的特效
        /// </summary>
        public List<string> ViewList;

        /// <summary>
        /// 锁定移动
        /// </summary>
        public bool LockMove;
    }
}