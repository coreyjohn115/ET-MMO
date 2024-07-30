using System.Collections.Generic;

namespace ET
{
    public partial class TaskConfigCategory
    {
        public Dictionary<TaskType, List<int>> TaskTypeToIds = new Dictionary<TaskType, List<int>>();

        public List<int> GetTaskList(TaskType type)
        {
            var list = new List<int>();
            if (this.TaskTypeToIds.TryGetValue(type, out var ids))
            {
                list.AddRange(ids);
            }

            return list;
        }

        public override void EndInit()
        {
            foreach (var config in this.dict)
            {
                var tt = config.Value.TaskType;
                if (!this.TaskTypeToIds.TryGetValue((TaskType)tt, out var list))
                {
                    list = new List<int>();
                    this.TaskTypeToIds.Add((TaskType)tt, list);
                }

                list.Add(config.Key);
            }
        }
    }

    public partial class TaskConfig
    {
        public List<CmdArg> GetConditionList { get; private set; }

        public List<CmdArg> CommitConditionList { get; private set; }

        public List<CmdArg> CommitCmdList { get; private set; }

        /// <summary>
        /// 任务对应活动ID
        /// </summary>
        public int ActivityId { get; private set; }

        public override void EndInit()
        {
            GetConditionList = this.GetConditionListStr.ParseCmdArgs();
            CommitConditionList = this.CommitConditionListStr.ParseCmdArgs();
            CommitCmdList = this.CommitCmdListStr.ParseCmdArgs();
        }
    }
}