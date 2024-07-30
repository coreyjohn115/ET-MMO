using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof (ClientTaskComponent))]
    [FriendOf(typeof (ClientTaskComponent))]
    public static partial class ClientTaskComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientTaskComponent self)
        {
            self.FinishTaskDict = new Dictionary<int, long>();
        }

        [EntitySystem]
        private static void Destroy(this ClientTaskComponent self)
        {
            self.FinishTaskDict.Clear();
        }

        private static void AddUpdateTask(this ClientTaskComponent self, TaskProto proto)
        {
            TaskUnit task = self.AddChildWithId<TaskUnit>(proto.Id);
            task.ToTask(proto);
            EventSystem.Instance.Publish(self.Scene(), new AddUpdateTask() { TaskUnit = task });
        }

        public static void AddUpdateTask(this ClientTaskComponent self, List<TaskProto> list)
        {
            foreach (var proto in list)
            {
                self.AddUpdateTask(proto);
            }
        }

        public static void UpdateFinishTask(this ClientTaskComponent self, Dictionary<int, long> finishMap)
        {
            foreach (var pair in finishMap)
            {
                self.FinishTaskDict.Add(pair.Key, pair.Value);
            }
        }

        public static void RemoveTask(this ClientTaskComponent self, List<int> list)
        {
            foreach (int taskId in list)
            {
                self.RemoveTask(taskId);
            }
        }

        public static void RemoveTask(this ClientTaskComponent self, int taskId)
        {
            TaskUnit task = self.GetChild<TaskUnit>();
            if (task)
            {
                EventSystem.Instance.Publish(self.Scene(), new DeleteTask() { TaskUnit = task });
            }

            self.FinishTaskDict.Remove(taskId);
        }

        public static bool TaskIsFinish(this ClientTaskComponent self, int taskId)
        {
            if (self.FinishTaskDict.TryGetValue(taskId, out _))
            {
                return true;
            }

            TaskUnit task = self.GetChild<TaskUnit>();
            if (!task)
            {
                return false;
            }

            return task.Status == TaskStatus.Commit;
        }
    }
}