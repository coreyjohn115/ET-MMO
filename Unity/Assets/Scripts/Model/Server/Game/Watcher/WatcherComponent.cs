using System.Collections.Generic;
using System.Diagnostics;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class WatcherComponent: Entity, IAwake, IDestroy
    {
        public long lastGcTime;
        public bool gg = false;
        public long Timer;
        public readonly Dictionary<int, Process> Processes = new();
    }
}