using System.Collections.Generic;

namespace ET
{
    public partial class TaskConfigCategory
    {
        public Dictionary<XTaskType, List<int>> TaskTypeToIds = new Dictionary<XTaskType, List<int>>();

        public List<int> GetTaskList(XTaskType type)
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
                if (!this.TaskTypeToIds.TryGetValue((XTaskType)tt, out var list))
                {
                    list = new List<int>();
                    this.TaskTypeToIds.Add((XTaskType)tt, list);
                }

                list.Add(config.Key);
            }
        }
    }

    public partial class TaskConfig
    {
        /// <summary>
        /// 任务对应活动ID
        /// </summary>
        public int ActivityId { get; private set; }
    }
}