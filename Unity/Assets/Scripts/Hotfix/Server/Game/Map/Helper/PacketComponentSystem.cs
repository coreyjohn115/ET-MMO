using System.Collections.Generic;
using System.Linq;

namespace ET.Server
{
    [EntitySystemOf(typeof (PacketComponent))]
    [FriendOf(typeof (PacketComponent))]
    public static partial class PacketComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PacketComponent self)
        {
            self.AddTasks = new List<TaskProto>(20);
            self.DelTasks = new List<int>(20);
            self.Timer = self.Scene().GetComponent<TimerComponent>().NewRepeatedTimer(200, TimerInvokeType.PacketUpdate, self);
        }

        [EntitySystem]
        private static void Destroy(this PacketComponent self)
        {
            self.AddTasks.Clear();
            self.DelTasks.Clear();
            self.Scene().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        [Invoke(TimerInvokeType.PacketUpdate)]
        private class PacketUpdate: ATimer<PacketComponent>
        {
            protected override void Run(PacketComponent self)
            {
                self.SyncPacket().NoContext();
            }
        }

        public static void UpdateTask(this PacketComponent self, TaskProto task)
        {
            self.AddTasks.Add(task);
        }

        public static void UpdateTask(this PacketComponent self, int id)
        {
            self.DelTasks.Add(id);
        }

        public static async ETTask SyncPacket(this PacketComponent self)
        {
            if (self.AddTasks.Count > 0)
            {
                var list = self.AddTasks.ToList();
                M2C_UpdateTask updateTask = M2C_UpdateTask.Create();
                updateTask.List.AddRange(list);
                await self.GetParent<Unit>().SendToClient(updateTask);
                self.AddTasks.Clear();
            }

            if (self.DelTasks.Count > 0)
            {
                var list = self.DelTasks.ToList();
                M2C_DeleteTask deleteTask = M2C_DeleteTask.Create();
                deleteTask.List.AddRange(list);
                await self.GetParent<Unit>().SendToClient(deleteTask);
                self.DelTasks.Clear();
            }
        }
    }
}