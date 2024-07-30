using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class RedDotComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<string, ListComponent<string>> RedDotNodeParentsDict = new();

        public HashSet<string> RedDotNodeNeedShowSet = new();

        public Dictionary<string, int> RetainViewCount = new();

        public Dictionary<string, string> ToParentDict = new();

        public Dictionary<string, int> RedDotNodeRetainCount = new();

        public Dictionary<string, RedDotMonoView> RedDotMonoViewDict = new();
    }
}