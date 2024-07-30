namespace ET
{
    public static class TaskDataSystem
    {
        public static TaskProto ToTaskProto(this TaskUnit self)
        {
            TaskProto proto = TaskProto.Create();
            proto.Id = (int)self.Id;
            proto.Args = self.Args;
            proto.Min = self.Min;
            proto.Max = self.Max;
            proto.Status = (int)self.Status;
            proto.Time = self.FinishTime;
            proto.AcceptTime = self.AcceptTime;
            return proto;
        }

        public static void ToTask(this TaskUnit self, TaskProto task)
        {
            self.Args = task.Args;
            self.Min = task.Min;
            self.Max = task.Max;
            self.Status = (TaskStatus)task.Status;
            self.FinishTime = task.Time;
            self.AcceptTime = task.AcceptTime;
        }
    }
}