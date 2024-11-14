using System;
using System.Collections.Generic;

namespace ET.Server
{
    [Code]
    public class TaskEffectSingleton: Singleton<TaskEffectSingleton>, ISingletonAwake
    {
        private Dictionary<string, ATaskArgs> taskArgDict;

        public Dictionary<string, ATaskArgs> TaskArgDict => taskArgDict;

        private Dictionary<string, ATaskHandler> taskHanderDict;

        public Dictionary<string, ATaskHandler> TaskHanderDict => taskHanderDict;

        private Dictionary<string, ATaskProcess> taskProcessDict;

        public Dictionary<string, ATaskProcess> TaskProcessDict => taskProcessDict;

        public void Awake()
        {
            taskArgDict = new Dictionary<string, ATaskArgs>();
            taskHanderDict = new Dictionary<string, ATaskHandler>();
            taskProcessDict = new Dictionary<string, ATaskProcess>();
            foreach (var v in CodeTypes.Instance.GetTypes(typeof (TaskArgsAttribute)))
            {
                var attr = v.GetCustomAttributes(typeof (TaskArgsAttribute), false)[0] as TaskArgsAttribute;
                taskArgDict.Add(attr.Key, Activator.CreateInstance(v) as ATaskArgs);
            }

            foreach (var v in CodeTypes.Instance.GetTypes(typeof (TaskHandlerAttribute)))
            {
                var attr = v.GetCustomAttributes(typeof (TaskHandlerAttribute), false)[0] as TaskHandlerAttribute;
                taskHanderDict.Add(attr.Key, Activator.CreateInstance(v) as ATaskHandler);
            }

            foreach (var v in CodeTypes.Instance.GetTypes(typeof (TaskProcessAttribute)))
            {
                var attr = v.GetCustomAttributes(typeof (TaskProcessAttribute), false)[0] as TaskProcessAttribute;
                taskProcessDict.Add(attr.Key, Activator.CreateInstance(v) as ATaskProcess);
            }
        }
    }
}