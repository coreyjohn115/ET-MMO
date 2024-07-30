using System;

namespace ET
{
    [EntitySystemOf(typeof (ItemData))]
    [FriendOf(typeof (ItemData))]
    public static partial class ItemDataSystem
    {
        [EntitySystem]
        private static void Awake(this ItemData self, int id)
        {
            self.cfgId = id;
            switch (self.ItemType)
            {
                case ItemType.Resource:
                    break;
                case ItemType.Normal:
                    self.AddComponent<NormalItemData>();
                    break;
                case ItemType.Equip:
                    self.AddComponent<EquipItemData>();
                    break;
                case ItemType.Pet:
                    self.AddComponent<PetItemData>();
                    break;
            }
        }

        [EntitySystem]
        private static void Destroy(this ItemData self)
        {
        }

        public static byte[] ToItemProto(this ItemData self)
        {
            return self.ToBson();
        }

        public static void FromProto(this ItemData self, ItemData proto)
        {
            self.Bind = proto.Bind;
            self.Count = proto.Count;
            self.ValidTime = proto.ValidTime;
            switch (self.ItemType)
            {
                case ItemType.Resource:
                    break;
                case ItemType.Normal:
                    break;
                case ItemType.Equip:
                    break;
                case ItemType.Pet:
                    break;
            }
        }
    }
}