using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 初始化任务
    /// </summary>
    public struct InitTask
    {
        public List<TaskUnit> List { get; set; }
    }

    [ComponentOf(typeof (Scene))]
    public class ClientTaskComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<int, long> FinishTaskDict;
    }
}