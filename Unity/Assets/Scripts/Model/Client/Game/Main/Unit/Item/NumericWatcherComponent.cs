using System;
using System.Collections.Generic;

namespace ET.Client
{
    public class ItemWatcherInfo
    {
        public IItemWatcher Watcher { get; }

        public ItemWatcherInfo(IItemWatcher watcher)
        {
            this.Watcher = watcher;
        }
    }

    /// <summary>
    /// 监视数值变化组件,分发监听
    /// </summary>
    [Code]
    public class ItemWatcherComponent: Singleton<ItemWatcherComponent>, ISingletonAwake
    {
        private readonly Dictionary<int, List<ItemWatcherInfo>> itemIdWatchers = new();
        private readonly Dictionary<ItemType, List<ItemWatcherInfo>> itemTypeWatchers = new();

        public void Awake()
        {
            HashSet<Type> idTypes = CodeTypes.Instance.GetTypes(typeof (ItemIdWatcherAttribute));
            foreach (Type type in idTypes)
            {
                object[] attrs = type.GetCustomAttributes(typeof (ItemIdWatcherAttribute), false);
                foreach (object attr in attrs)
                {
                    ItemIdWatcherAttribute watcherAttribute = (ItemIdWatcherAttribute)attr;
                    IItemWatcher obj = (IItemWatcher)Activator.CreateInstance(type);
                    ItemWatcherInfo watcherInfo = new(obj);
                    if (!itemIdWatchers.TryGetValue(watcherAttribute.CfgId, out List<ItemWatcherInfo> value))
                    {
                        value = new List<ItemWatcherInfo>();
                        this.itemIdWatchers.Add(watcherAttribute.CfgId, value);
                    }

                    value.Add(watcherInfo);
                }
            }

            HashSet<Type> typeTypes = CodeTypes.Instance.GetTypes(typeof (ItemTypeWatcherAttribute));
            foreach (Type type in typeTypes)
            {
                object[] attrs = type.GetCustomAttributes(typeof (ItemTypeWatcherAttribute), false);
                foreach (object attr in attrs)
                {
                    ItemTypeWatcherAttribute watcherAttribute = (ItemTypeWatcherAttribute)attr;
                    IItemWatcher obj = (IItemWatcher)Activator.CreateInstance(type);
                    ItemWatcherInfo watcherInfo = new(obj);
                    if (!itemTypeWatchers.TryGetValue(watcherAttribute.ItemType, out List<ItemWatcherInfo> value))
                    {
                        value = new List<ItemWatcherInfo>();
                        this.itemTypeWatchers.Add(watcherAttribute.ItemType, value);
                    }

                    value.Add(watcherInfo);
                }
            }
        }

        public void Run(ItemData item, RemoveItemEvent removeEvent)
        {
            if (this.itemIdWatchers.TryGetValue(item.Config.Id, out var list))
            {
                foreach (ItemWatcherInfo info in list)
                {
                    info.Watcher.Remove(removeEvent);
                }
            }

            if (this.itemTypeWatchers.TryGetValue(item.ItemType, out var list2))
            {
                foreach (ItemWatcherInfo info in list2)
                {
                    info.Watcher.Remove(removeEvent);
                }
            }
        }

        public void Run(ItemData item, AddItemEvent addEvent)
        {
            if (this.itemIdWatchers.TryGetValue(item.Config.Id, out var list))
            {
                foreach (ItemWatcherInfo info in list)
                {
                    info.Watcher.Add(addEvent);
                }
            }

            if (this.itemTypeWatchers.TryGetValue(item.ItemType, out var list2))
            {
                foreach (ItemWatcherInfo info in list2)
                {
                    info.Watcher.Add(addEvent);
                }
            }
        }
    }
}