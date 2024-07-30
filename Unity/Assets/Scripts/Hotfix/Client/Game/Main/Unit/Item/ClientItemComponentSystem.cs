using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof (ClientItemComponent))]
    [FriendOf(typeof (ClientItemComponent))]
    public static partial class ClientItemComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientItemComponent self)
        {
        }

        public static void AddUpdateItem(this ClientItemComponent self, long id, ItemData proto)
        {
            long oldCount = 0L;
            if (!self.HasChild(id))
            {
                self.AddChild(proto);
            }
            else
            {
                ItemData child = self.GetChild<ItemData>(id);
                oldCount = child.Count;
                child.FromProto(proto);
                proto.Dispose();
            }

            EventSystem.Instance.Publish(self.Scene(),
                new AddItemEvent() { Item = self.GetChild<ItemData>(id), ChangeCount = proto.Count - oldCount });
        }

        public static void RemoveItem(this ClientItemComponent self, long id)
        {
            ItemData child = self.GetChild<ItemData>(id);
            if (!child)
            {
                return;
            }

            EventSystem.Instance.Publish(self.Scene(), new RemoveItemEvent() { Item = child, Count = child.Count });
        } 

        /// <summary>
        /// 初始化玩家道具
        /// </summary>
        public static void AddUpdateItem(this ClientItemComponent self, List<byte[]> list)
        {
            foreach (byte[] item in list)
            {
                ItemData d = MongoHelper.Deserialize<ItemData>(item);
                self.AddUpdateItem(d.Id, d);
            }
        }

        public static ItemData GetItem(this ClientItemComponent self, long id)
        {
            return self.GetChild<ItemData>(id);
        }
    }
}