using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 添加任务
    /// </summary>
    public struct AddUpdateTask
    {
        public EntityRef<TaskUnit> TaskUnit { get; set; }
    }

    /// <summary>
    /// 完成任务
    /// </summary>
    public struct FinishTask
    {
        public EntityRef<TaskUnit> TaskUnit { get; set; }
    }

    /// <summary>
    /// 提交任务
    /// </summary>
    public struct CommitTask
    {
        public EntityRef<TaskUnit> TaskUnit { get; set; }
    }

    /// <summary>
    /// 删除任务
    /// </summary>
    public struct DeleteTask
    {
        public EntityRef<TaskUnit> TaskUnit { get; set; }
    }
}