using System.Collections.Generic;

namespace ET.Server
{
    public class TaskHandlerAttribute: BaseAttribute
    {
        public string Key { get; }

        public TaskHandlerAttribute(string key)
        {
            this.Key = key;
        }
    }

    public abstract class ATaskHandler
    {
        public abstract bool Run(TaskComponent self, TaskUnit task, List<long> args, long[] cfgArgs);
    }
}