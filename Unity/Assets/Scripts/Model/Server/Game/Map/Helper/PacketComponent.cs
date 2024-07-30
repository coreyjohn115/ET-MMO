using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof (Unit))]
    public class PacketComponent: Entity, IAwake, IDestroy
    {
        public List<TaskProto> AddTasks;
        public List<int> DelTasks;

        public long Timer;
    }
}