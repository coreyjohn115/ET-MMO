using System;

namespace ET.Client
{
    public interface IItemWatcher
    {
        void Add(AddItemEvent args);

        void Remove(RemoveItemEvent args);
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ItemIdWatcherAttribute: BaseAttribute
    {
        public int CfgId { get; }

        public ItemIdWatcherAttribute(int type)
        {
            this.CfgId = type;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ItemTypeWatcherAttribute: BaseAttribute
    {
        public ItemType ItemType { get; }

        public ItemTypeWatcherAttribute(ItemType type)
        {
            this.ItemType = type;
        }
    }
}