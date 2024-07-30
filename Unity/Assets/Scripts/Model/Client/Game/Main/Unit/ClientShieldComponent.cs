using System.Collections.Generic;

namespace ET.Client
{
    public struct UpdateShield
    {
        public Unit Unit;
    }

    [ComponentOf(typeof (Unit))]
    public class ClientShieldComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<int, long> shieldIdDict = new();
    }
}