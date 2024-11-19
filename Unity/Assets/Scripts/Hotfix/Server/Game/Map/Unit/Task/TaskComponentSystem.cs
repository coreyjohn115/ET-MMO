using System;
using System.Collections.Generic;
using System.Linq;

namespace ET.Server
{
    [EntitySystemOf(typeof (TaskComponent))]
    [FriendOf(typeof (TaskComponent))]
    public static partial class TaskComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TaskComponent self)
        {
            self.Load();
            self.InitTask();
        }

        [EntitySystem]
        private static void Destroy(this TaskComponent self)
        {
        }

        [EntitySystem]
        private static void Load(this TaskComponent self)
        {
            self.finishTaskDict = new Dictionary<int, FinishTaskData>();

            //为了可以重载放在Task组件上
            self.taskFuncDict = new Dictionary<TaskEventType, TaskFunc>()
            {
                { TaskEventType.Complete, new TaskFunc("Default", Pair<int, int>.Create(0, 0)) },
                { TaskEventType.UseCountItem, new TaskFunc("Common2", Pair<int, int>.Create(1, 1)) },
                { TaskEventType.AddCountItem, new TaskFunc("Common2", Pair<int, int>.Create(1, 1)) },
                { TaskEventType.ConsumeCountItem, new TaskFunc("Common2", Pair<int, int>.Create(1, 1)) },
                { TaskEventType.HomeLevel, new TaskFunc("Common1", Pair<int, int>.Create(1, 1)) },
            };
        }

        private static void InitTask(this TaskComponent self)
        {
            var list = TaskConfigCategory.Instance.GetTaskList(XTaskType.HomeAchievement);
            foreach (int id in list)
            {
                if (!self.HasTask(id))
                {
                    self.AddTask(id, new AddTaskData() { LogEvent = LogDef.TaskInit, NotUpdate = true });
                }
            }
        }

        private static void UpdateTask(this TaskComponent self, TaskUnit task)
        {
            var proto = task.ToTaskProto();
            if (self.taskFuncDict.TryGetValue((TaskEventType)task.Config.EventType, out var tf))
            {
                var ff = TaskEffectSingleton.Instance.TaskProcessDict[tf.TaskProcess];
                var pro = ff.Run(self, task, task.Config.Args);
                proto.Min = pro.Key;
                proto.Max = pro.Value;
            }

            self.GetParent<Unit>().GetComponent<PacketComponent>().UpdateTask(proto);
        }

        public static void CheckTask(this TaskComponent self)
        {
            bool updateCache = false;
            foreach (var task in self.Children.Values)
            {
                if (TaskConfigCategory.Instance.Contain((int)task.Id))
                {
                    continue;
                }

                updateCache = true;
                Log.Info($"任务因配置变化而删除: {task.Id}");
                self.DelTask((int)task.Id, LogDef.TaskConfigRemove);
            }

            foreach (int id in self.finishTaskDict.Keys)
            {
                if (TaskConfigCategory.Instance.Contain(id))
                {
                    continue;
                }

                updateCache = true;
                Log.Info($"完成任务因配置变化而删除: {id}");
                self.finishTaskDict.Remove(id);
            }

            if (updateCache)
            {
                self.UpdateCache().NoContext();
            }
        }

        public static TaskUnit GetTask(this TaskComponent self, int taskId)
        {
            TaskUnit task = self.GetChild<TaskUnit>(taskId);
            return task;
        }

        public static Pair<Dictionary<int, long>, List<TaskProto>> GetTaskList(this TaskComponent self)
        {
            var list = new List<TaskProto>();
            foreach (TaskUnit value in self.Children.Values)
            {
                list.Add(value.ToTaskProto());
            }

            var finishDict = self.finishTaskDict.ToDictionary(pair => pair.Key, pair => pair.Value.FinishTime);

            return new Pair<Dictionary<int, long>, List<TaskProto>>(finishDict, list);
        }

        public static bool HasTask(this TaskComponent self, int taskId)
        {
            TaskUnit task = self.GetChild<TaskUnit>(taskId);
            if (task)
            {
                return true;
            }

            if (self.finishTaskDict.TryGetValue(taskId, out _))
            {
                return true;
            }

            return false;
        }

        private static bool ProcessArgs(this TaskComponent self, TaskUnit task, List<long> args)
        {
            if (!self.taskFuncDict.TryGetValue((TaskEventType)task.Config.EventType, out var tf))
            {
                return false;
            }

            if (!TaskEffectSingleton.Instance.TaskArgDict.TryGetValue(tf.TaskArgs, out var ff))
            {
                Log.Error($"不存在处理函数: {tf.TaskArgs}");
                return false;
            }

            bool ok = ff.Run(self, task, args, task.Config.Args);
            if (ok)
            {
                if (task.Config.ShopProgress)
                {
                    self.UpdateTask(task);
                }
            }

            return ok;
        }

        private static void ProcessHandler(this TaskComponent self, TaskUnit task)
        {
            if (!self.taskFuncDict.TryGetValue((TaskEventType)task.Config.EventType, out var tf))
            {
                return;
            }

            if (!TaskEffectSingleton.Instance.TaskHandeDict.TryGetValue(tf.TaskHandle, out var ff))
            {
                Log.Error($"不存在处理函数: {tf.TaskHandle}");
                return;
            }

            bool ok = ff.Run(self, task, task.Args, task.Config.Args);
            if (ok)
            {
                self.FinishTask((int)task.Id);
            }
        }

        private static void ProcessTask(this TaskComponent self, TaskUnit task, List<long> args = default)
        {
            args ??= new List<long>();
            if (task.Status == TaskStatus.Accept)
            {
                bool isChange = self.ProcessArgs(task, args);
                if (isChange)
                {
                    self.ProcessHandler(task);
                }
            }
        }

        /// <summary>
        /// 处理任务事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eventType"></param>
        /// <param name="args"></param>
        public static void ProcessTaskEvent(this TaskComponent self, TaskEventType eventType, List<long> args = default)
        {
            var list = new List<int>();
            int evt = (int)eventType;
            foreach ((long key, Entity value) in self.Children)
            {
                TaskUnit task = value as TaskUnit;
                if (task.Config.EventType == evt)
                {
                    list.Add((int)key);
                }
            }

            foreach (int id in list)
            {
                TaskUnit task = self.GetChild<TaskUnit>(id);
                if (task)
                {
                    self.ProcessTask(task, args);
                }
            }
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="self"></param>
        /// <param name="taskId">任务ID</param>
        /// <param name="data">任务数据</param>
        /// <returns></returns>
        public static MessageReturn AddTask(this TaskComponent self, int taskId, AddTaskData data)
        {
            if (data.Replace)
            {
                self.DelTask(taskId, LogDef.TaskAdd);
            }
            else
            {
                TaskUnit task = self.GetChild<TaskUnit>(taskId);
                if (task)
                {
                    return MessageReturn.Success();
                }

                var taskCfg = TaskConfigCategory.Instance.Get(taskId);
                var ret = Cmd.Instance.ProcessCmdList(self.GetParent<Unit>(), taskCfg.GetConditionList);
                if (ret.Errno != ErrorCode.ERR_Success)
                {
                    return ret;
                }

                task = self.AddChildWithId<TaskUnit>(taskId);
                task.Status = TaskStatus.Accept;
                task.AcceptTime = TimeInfo.Instance.Frame;
                EventSystem.Instance.Publish(self.Scene(), new AddUpdateTask() { TaskUnit = task });
                if (!data.NotUpdate)
                {
                    self.UpdateTask(task);
                }

                self.ProcessTask(task);
            }

            return MessageReturn.Success();
        }

        /// <summary>
        /// 删除玩家任务
        /// </summary>
        /// <param name="self"></param>
        /// <param name="taskId">任务ID</param>
        /// <param name="logEvent">来源日志</param>
        public static void DelTask(this TaskComponent self, int taskId, int logEvent)
        {
            TaskUnit task = self.GetChild<TaskUnit>(taskId);
            if (!task)
            {
                return;
            }

            EventSystem.Instance.Publish(self.Scene(), new DeleteTask() { TaskUnit = task });
            self.GetParent<Unit>().GetComponent<PacketComponent>().UpdateTask(taskId);
            self.RemoveChild(taskId);
        }

        private static MessageReturn PlayerTaskCommit(this TaskComponent self, TaskUnit task, int logEvent)
        {
            switch (task.Status)
            {
                case TaskStatus.Accept:
                    return MessageReturn.Create(ErrorCode.ERR_TaskNotFinish);
                case TaskStatus.Commit:
                    return MessageReturn.Create(ErrorCode.ERR_TaskIsCommit);
                case TaskStatus.Timeout:
                    return MessageReturn.Create(ErrorCode.ERR_TaskIsTimeOut);
            }

            var ret = Cmd.Instance.ProcessCmdList(self.GetParent<Unit>(), task.Config.CommitConditionList);
            if (ret.Errno != ErrorCode.ERR_Success)
            {
                return ret;
            }

            //计算奖励
            task.Status = TaskStatus.Commit;
            task.CommitTime = TimeInfo.Instance.Frame;
            self.UpdateTask(task);
            EventSystem.Instance.Publish(self.Scene(), new CommitTask() { TaskUnit = task });
            if (!task.Config.FinishShow)
            {
                self.DelTask((int)task.Id, LogDef.TaskCommit);
            }

            if (task.Config.IsEnterFinish)
            {
                self.finishTaskDict.Add((int)task.Id, new FinishTaskData() { Args = task.Args, FinishTime = task.CommitTime });
            }

            foreach (int id in task.Config.NextList)
            {
                self.AddTask(id, new AddTaskData() { LogEvent = LogDef.TaskCommit });
            }

            Cmd.Instance.ProcessCmdList(self.GetParent<Unit>(), task.Config.CommitCmdList, new List<long>() { task.Id }, true);

            return MessageReturn.Success();
        }

        private static MessageReturn LeagueTaskCommit(this TaskComponent self, TaskUnit task, int logEvent)
        {
            if (self.TaskIsCommit((int)task.Id))
            {
                return MessageReturn.Create(ErrorCode.ERR_TaskIsCommit);
            }

            return MessageReturn.Success();
        }

        private static MessageReturn ServerTaskCommit(this TaskComponent self, TaskUnit task, int logEvent)
        {
            if (self.TaskIsCommit((int)task.Id))
            {
                return MessageReturn.Create(ErrorCode.ERR_TaskIsCommit);
            }

            return MessageReturn.Success();
        }

        /// <summary>
        /// 提交任务
        /// </summary>
        /// <param name="self"></param>
        /// <param name="taskId"></param>
        /// <param name="logEvent"></param>
        /// <returns></returns>
        public static MessageReturn CommitTask(this TaskComponent self, int taskId, int logEvent)
        {
            TaskUnit task = self.GetChild<TaskUnit>(taskId);
            if (!task)
            {
                //可能有重复提交
                return MessageReturn.Success();
            }

            switch (task.ObjType)
            {
                case TaskObjType.Player:
                    return self.PlayerTaskCommit(task, logEvent);
                case TaskObjType.League:
                    return self.LeagueTaskCommit(task, logEvent);
                case TaskObjType.Server:
                    return self.ServerTaskCommit(task, logEvent);
            }

            return MessageReturn.Success();
        }

        /// <summary>
        /// 任务超时
        /// </summary>
        /// <param name="self"></param>
        /// <param name="taskId"></param>
        /// <param name="logEvent"></param>
        /// <returns></returns>
        public static int TimeoutTask(this TaskComponent self, int taskId, int logEvent)
        {
            TaskUnit task = self.GetChild<TaskUnit>(taskId);
            if (!task)
            {
                return ErrorCode.ERR_CantFindCfg;
            }

            task.Status = TaskStatus.Timeout;
            self.UpdateTask(task);
            return ErrorCode.ERR_Success;
        }

        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="self"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public static MessageReturn FinishTask(this TaskComponent self, int taskId)
        {
            TaskUnit task = self.GetChild<TaskUnit>(taskId);
            if (!task)
            {
                return MessageReturn.Success();
            }

            task.Args[0] = task.Config.Args[0];
            task.Status = TaskStatus.Finish;
            task.FinishTime = TimeInfo.Instance.Frame;
            EventSystem.Instance.Publish(self.Scene(), new FinishTask() { TaskUnit = task });
            self.UpdateTask(task);
            if (task.Config.AutoCommit)
            {
                self.CommitTask(taskId, LogDef.TaskAutoCommit);
            }

            return MessageReturn.Success();
        }

        /// <summary>
        /// 任务是否提交
        /// </summary>
        /// <param name="self"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public static bool TaskIsCommit(this TaskComponent self, int taskId)
        {
            if (self.finishTaskDict.TryGetValue(taskId, out _))
            {
                return true;
            }

            TaskUnit task = self.GetChild<TaskUnit>(taskId);
            if (!task)
            {
                return false;
            }

            return task.Status == TaskStatus.Commit;
        }

        /// <summary>
        /// 任务是否完成
        /// </summary>
        /// <param name="self"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public static bool TaskIsFinish(this TaskComponent self, int taskId)
        {
            TaskUnit task = self.GetChild<TaskUnit>(taskId);
            if (!task)
            {
                return false;
            }

            return task.Status is TaskStatus.Finish or TaskStatus.Commit;
        }

        public static void UpdateTaskStatus(this TaskComponent self, int taskId, TaskStatus status)
        {
            TaskUnit task = self.GetChild<TaskUnit>(taskId);
            if (!task)
            {
                return;
            }

            task.Status = status;
            self.UpdateTask(task);
        }

        public static void SetTaskArgs(this TaskComponent self, int taskId, List<long> args)
        {
            TaskUnit task = self.GetChild<TaskUnit>(taskId);
            if (!task)
            {
                return;
            }

            task.Args = args;
            self.UpdateTask(task);
        }
    }
}