namespace ET
{
    /// <summary>
    /// 添加更新道具事件
    /// </summary>
    public struct AddItemEvent
    {
        public EntityRef<Unit> Unit { get; set; }

        public EntityRef<ItemData> Item { get; set; }

        public long ChangeCount { get; set; }

        public int LogEvent { get; set; }
    }

    /// <summary>
    /// 移除道具事件
    /// </summary>
    public struct RemoveItemEvent
    {
        public EntityRef<Unit> Unit { get; set; }

        public EntityRef<ItemData> Item { get; set; }

        public long Count { get; set; }

        public int LogEvent { get; set; }
    }
}