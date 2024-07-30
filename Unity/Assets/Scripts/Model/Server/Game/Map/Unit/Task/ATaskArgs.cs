using System.Collections.Generic;

namespace ET.Server
{
    public class TaskArgsAttribute: BaseAttribute
    {
        public string Key { get; }

        public TaskArgsAttribute(string key)
        {
            this.Key = key;
        }
    }

    public abstract class ATaskArgs
    {
        public abstract bool Run(TaskComponent self, TaskUnit task, List<long> args, long[] cfgArgs);
    }
}