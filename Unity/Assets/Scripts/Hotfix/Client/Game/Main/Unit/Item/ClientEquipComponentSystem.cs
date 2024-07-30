using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof (ClientEquipComponent))]
    [FriendOf(typeof (ClientEquipComponent))]
    public static partial class ClientEquipComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientEquipComponent self)
        {
        }

        public static void UpdateEquip(this ClientEquipComponent self, Dictionary<EquipPosType, long> equipDict)
        {
            self.equipDict.Clear();
            foreach ((EquipPosType key, long value) in equipDict)
            {
                self.equipDict.Add(key, value);
                EventSystem.Instance.Publish(self.Scene(), new EquipUpdateEvent() { EquipPosType = key, EquipId = value });
            }
        }

        public static long GetEquip(this ClientEquipComponent self, EquipPosType equipPosType)
        {
            return self.equipDict.GetValueOrDefault(equipPosType);
        }
    }
}