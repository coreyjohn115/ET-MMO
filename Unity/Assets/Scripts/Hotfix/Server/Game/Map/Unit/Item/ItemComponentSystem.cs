using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ET.Server;

[EntitySystemOf(typeof (ItemComponent))]
[FriendOf(typeof (ItemComponent))]
[FriendOf(typeof (ItemData))]
public static partial class ItemComponentSystem
{
    [Event(SceneType.Map)]
    public class UnitCheckCfg_CheckItem: AEvent<Scene, UnitCheckCfg>
    {
        protected override async ETTask Run(Scene scene, UnitCheckCfg a)
        {
            a.Unit.GetComponent<ItemComponent>().CheckItem();
            await ETTask.CompletedTask;
        }
    }

    [EntitySystem]
    private static void Awake(this ItemComponent self)
    {
    }

    [EntitySystem]
    private static void Destroy(this ItemComponent self)
    {
    }

    /*----------------------------------------------------------------------------------------------------------------*/

    public static List<byte[]> GetItemList(this ItemComponent self)
    {
        var list = new List<byte[]>();
        foreach (ItemData value in self.Children.Values)
        {
            list.Add(value.ToItemProto());
        }

        return list;
    }

    public static void AddItemListStr(this ItemComponent self, string itemListStr, int logEvent)
    {
        List<ItemArgs> itemList = itemListStr.ParseItemArgs();
        self.AddItemList(itemList, new AddItemData() { LogEvent = logEvent });
    }

    public static void AddItemList(this ItemComponent self, List<ItemArgs> itemList, AddItemData data)
    {
        if (itemList.IsNullOrEmpty())
        {
            return;
        }

        Thrower.IsTrue(data.LogEvent != 0, "非法日志类型");
        foreach (ItemArgs arg in itemList)
        {
            if (arg.Count <= 0L)
            {
                continue;
            }

            IItemConfig config = XItemConfigCategory.Instance.GetConfig(arg.Id);
            if (config == null)
            {
                continue;
            }

            switch (config.Stack)
            {
                // 数量不限
                case < 0:
                    ItemData item = self.FindItem(arg.Id);
                    if (!item)
                    {
                        item = self.AddItem(arg.Id);
                    }

                    item.Count += arg.Count;
                    self.PublishAddEvent(item, arg.Count, data.LogEvent);
                    if (!data.NotUpdate)
                    {
                        self.UpdateItem(item);
                    }

                    break;
                case 0:
                case 1:
                    _AddItem(arg.Count, arg, config);
                    break;
                default:
                    item = self.FindItem(arg.Id);
                    if (!item)
                    {
                        item = self.AddItem(arg.Id);
                    }

                    if (item.Count + arg.Count > config.Stack)
                    {
                        item.Count = config.Stack;
                        long remainC = arg.Count - (config.Stack - item.Count);
                        long c = remainC % config.Stack == 0L? remainC / config.Stack : (remainC / config.Stack + 1L);
                        _AddItem(c, arg, config);
                    }
                    else
                    {
                        item.Count += arg.Count;
                    }

                    self.PublishAddEvent(item, arg.Count, data.LogEvent);
                    if (!data.NotUpdate)
                    {
                        self.UpdateItem(item);
                    }

                    break;
            }
        }

        return;

        void _AddItem(long c, ItemArgs arg, IItemConfig config)
        {
            for (int i = 0; i < c; i++)
            {
                ItemData it = self.AddItem(arg.Id);
                it.Count = config.Stack;
                self.PublishAddEvent(it, it.Count, data.LogEvent);
                if (!data.NotUpdate)
                {
                    self.UpdateItem(it);
                }
            }
        }
    }

    /// <summary>
    /// 消耗道具列表
    /// </summary>
    /// <param name="self"></param>
    /// <param name="itemListStr"></param>
    /// <param name="logEvent"></param>
    /// <returns></returns>
    public static MessageReturn ConsumeItemListStr(this ItemComponent self, string itemListStr, int logEvent)
    {
        List<ItemArgs> itemList = itemListStr.ParseItemArgs();
        return self.ConsumeItemList(itemList, new AddItemData() { LogEvent = logEvent });
    }

    /// <summary>
    /// 消耗道具
    /// </summary>
    /// <param name="self"></param>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static MessageReturn RemoveItem(this ItemComponent self, long id, AddItemData data)
    {
        Thrower.IsTrue(data.LogEvent != 0, "非法日志类型");
        var item = self.GetChild<ItemData>(id);
        if (!item)
        {
            return MessageReturn.Success();
        }

        self.RemoveChild(id);
        if (self.cfgIdDict.TryGetValue(item.cfgId, out var set))
        {
            set.Remove(id);
            if (set.Count == 0)
            {
                self.cfgIdDict.Remove(item.cfgId);
            }
        }

        self.validItemDict.Remove(id);
        self.PublishRemoveEvent(item, item.Count, data.LogEvent);
        if (data.NotUpdate)
        {
            return MessageReturn.Success();
        }

        M2C_UpdateItem updateItem = M2C_UpdateItem.Create();
        updateItem.Id = item.Id;
        updateItem.IsDelete = true;
        self.GetParent<Unit>().SendToClient(updateItem).NoContext();

        return MessageReturn.Success();
    }

    /// <summary>
    /// 消耗道具列表
    /// </summary>
    /// <param name="self">道具组件</param>
    /// <param name="itemList"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static MessageReturn ConsumeItemList(this ItemComponent self, List<ItemArgs> itemList, AddItemData data)
    {
        var ret = self.ItemEnough(itemList);
        if (ret.Errno != ErrorCode.ERR_Success)
        {
            return ret;
        }

        foreach (ItemArgs arg in itemList)
        {
            if (arg.Count <= 0)
            {
                continue;
            }

            long count = arg.Count;
            var set = self.cfgIdDict[arg.Id];
            foreach (long l in set)
            {
                ItemData item = self.GetChild<ItemData>(l);
                if (item.Count >= count)
                {
                    item.Count -= count;
                    if (item.Count <= 0L)
                    {
                        self.RemoveItem(item.Id, data);
                    }
                    else
                    {
                        self.PublishRemoveEvent(item, count, data.LogEvent);
                        if (!data.NotUpdate)
                        {
                            self.UpdateItem(item);
                        }
                    }

                    break;
                }

                count -= item.Count;
                self.RemoveItem(item.Id, data);
                if (count <= 0)
                {
                    break;
                }
            }
        }

        return MessageReturn.Success();
    }

    /// <summary>
    /// 道具数量是否足够
    /// </summary>
    /// <param name="self">道具组件</param>
    /// <param name="itemList"></param>
    /// <returns></returns>
    public static MessageReturn ItemEnough(this ItemComponent self, List<ItemArgs> itemList)
    {
        if (itemList.IsNullOrEmpty())
        {
            return MessageReturn.Success();
        }

        var map = new Dictionary<int, long>();
        map = itemList.GroupBy(v => v.Id).ToDictionary(v => v.Key, v => v.Sum(v1 => v1.Count));
        foreach ((int itemId, long count) in map)
        {
            if (count <= 0)
            {
                continue;
            }

            long owenCount = self.GetItemCount(itemId);
            if (owenCount < count)
            {
                return MessageReturn.Create(ErrorCode.ERR_ItemNotEnough, MiscHelper.GetItemError(itemId));
            }
        }

        return MessageReturn.Success();
    }

    /// <summary>
    /// 根据道具唯一ID获取道具实例
    /// </summary>
    /// <param name="self"></param>
    /// <param name="id"></param>
    /// <returns>道具实例</returns>
    public static ItemData GetItem(this ItemComponent self, long id)
    {
        var child = self.GetChild<ItemData>(id);
        return child;
    }

    /// <summary>
    /// 根据道具唯一ID获取道具数量
    /// </summary>
    /// <param name="self">道具组件</param>
    /// <param name="id">道具唯一ID</param>
    /// <returns>道具数量</returns>
    public static long GetItemCount(this ItemComponent self, long id)
    {
        var item = self.GetChild<ItemData>(id);
        if (item)
        {
            return item.Count;
        }

        return 0;
    }

    /// <summary>
    /// 根据配置ID获取道具数量
    /// </summary>
    /// <param name="self">道具组件</param>
    /// <param name="cfgId">道具配置ID</param>
    /// <returns>道具数量</returns>
    public static long GetItemCount(this ItemComponent self, int cfgId)
    {
        if (!self.cfgIdDict.TryGetValue(cfgId, out var set))
        {
            return 0L;
        }

        long count = 0L;
        foreach (long l in set)
        {
            ItemData item = self.GetChild<ItemData>(l);
            count += item.Count;
        }

        return count;
    }

    /*----------------------------------------------------------------------------------------------------------------*/

    private static void CheckItem(this ItemComponent self)
    {
        //检测默认道具
        var list = new List<ItemArgs>();
        bool updateCache = false;
        foreach (var config in InitItemConfigCategory.Instance.GetAll())
        {
            if (!self.initItemIds.Contains(config.Key))
            {
                list.Add(new ItemArgs() { Id = config.Key, Count = config.Value.Count });
                self.initItemIds.Add(config.Key);
                updateCache = true;
            }
        }

        if (list.Count > 0)
        {
            self.AddItemList(list, new AddItemData() { NotUpdate = true, LogEvent = LogDef.ItemInit });
        }

        //道具ID检测
        foreach (ItemData itemData in self.Children.Values)
        {
            if (XItemConfigCategory.Instance.Contain(itemData.cfgId))
            {
                continue;
            }

            updateCache = true;
            Log.Warning($"道具因配置变化而删除, 道具ID: {itemData.Id} {itemData.cfgId}");
            self.validItemDict.Remove(itemData.Id);
            self.RemoveItem(itemData.Id, new AddItemData() { LogEvent = LogDef.ItemConfigRemove, NotUpdate = true });
        }

        if (updateCache)
        {
            self.UpdateCache().NoContext();
        }
    }

    private static void UpdateItem(this ItemComponent self, ItemData item)
    {
        M2C_UpdateItem updateItem = M2C_UpdateItem.Create();
        updateItem.Id = item.Id;
        updateItem.Item = item.ToItemProto();
        self.GetParent<Unit>().SendToClient(updateItem).NoContext();
    }

    private static void PublishAddEvent(this ItemComponent self, ItemData item, long count, int logEvent)
    {
        EventSystem.Instance.Publish(self.Scene(),
            new AddItemEvent() { Unit = self.GetParent<Unit>(), Item = item, ChangeCount = count - item.Count, LogEvent = logEvent });
    }

    private static void PublishRemoveEvent(this ItemComponent self, ItemData item, long count, int logEvent)
    {
        EventSystem.Instance.Publish(self.Scene(),
            new RemoveItemEvent() { Unit = self.GetParent<Unit>(), Item = item, Count = count, LogEvent = logEvent });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ItemData FindItem(this ItemComponent self, int cfgId)
    {
        IItemConfig cc = XItemConfigCategory.Instance.GetConfig(cfgId);
        if (!self.cfgIdDict.TryGetValue(cfgId, out var set))
        {
            return default;
        }

        foreach (long l in set)
        {
            ItemData item = self.GetChild<ItemData>(l);
            if (cc.Stack == -1 || (cc.Stack > 1 && item.Count < cc.Stack))
            {
                return item;
            }
        }

        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ItemData AddItem(this ItemComponent self, int id)
    {
        var item = self.AddChild<ItemData, int>(id);
        if (!self.cfgIdDict.TryGetValue(id, out var set))
        {
            set = [];
            self.cfgIdDict.Add(id, set);
        }

        set.Add(item.Id);
        return item;
    }
}