using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class BattleText: Entity, IAwake, IDestroy
    {
        public long timer;

        public HashSet<string> hudNameSet = new HashSet<string>();
        public Dictionary<long, List<Pair<long, string>>> waitPopDict = new Dictionary<long, List<Pair<long, string>>>();
    }
}