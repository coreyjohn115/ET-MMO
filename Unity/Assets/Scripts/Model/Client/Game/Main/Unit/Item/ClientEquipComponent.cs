using System.Collections.Generic;

namespace ET.Client
{
    public struct EquipUpdateEvent
    {
        public EquipPosType EquipPosType;
        public long EquipId;
    }

    [ComponentOf(typeof (Scene))]
    public class ClientEquipComponent: Entity, IAwake
    {
        public Dictionary<EquipPosType, long> equipDict = new();
    }
}