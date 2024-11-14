using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server
{
    public struct AddTaskData
    {
        public int LogEvent { get; set; }

        public bool Replace { get; set; }

        public bool NotUpdate { get; set; }
    }

    public class FinishTaskData : Object
    {
        public List<long> Args { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public long FinishTime { get; set; }
    }

    public struct TaskFunc
    {
        public TaskFunc(string taskArgs, Pair<int, int> process, string taskHandle = "Default", string taskProcess = "Default")
        {
            this.TaskArgs = taskArgs;
            this.Process = process;
            this.TaskHandle = taskHandle;
            this.TaskProcess = taskProcess;
        }

        public Pair<int, int> Process { get; }
        public string TaskArgs { get; }
        public string TaskHandle { get; }
        public string TaskProcess { get; }
    }

    [UnitCom]
    [ComponentOf(typeof (Unit))]
    public class TaskComponent: Entity, IAwake, IDestroy, ILoad, ICache, ITransfer
    {
        [BsonIgnore]
        public Dictionary<TaskEventType, TaskFunc> taskFuncDict;

        /// <summary>
        /// 完成任务字典
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, FinishTaskData> finishTaskDict;
    }
}