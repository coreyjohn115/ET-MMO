using System.Collections.Generic;

namespace ET.Client
{
    public struct AddBuffView
    {
        public Unit Unit;

        public string ViewCmd;
    }

    public struct RemoveBuffView
    {
        public Unit Unit;

        public string ViewCmd;
    }

    [ComponentOf(typeof (Unit))]
    public class ClientBuffComponent: Entity, IAwake
    {
        public Dictionary<string, HashSet<long>> buffListDict = new();
    }
}