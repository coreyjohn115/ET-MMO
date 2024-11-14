using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 默认成功
    /// </summary>
    [TaskProcess("Default")]
    public class TaskProcessDefault: ATaskProcess
    {
        public override Pair<long, long> Run(TaskComponent self, TaskUnit task, long[] cfgArgs)
        {
            return new Pair<long, long>(task.Min, task.Max);
        }
    }

    [TaskProcess("SubTask")]
    public class TaskProcessSubTask: ATaskProcess
    {
        public override Pair<long, long> Run(TaskComponent self, TaskUnit task, long[] cfgArgs)
        {
            return new Pair<long, long>(task.Args[0], cfgArgs.Length - 1);
        }
    }
}