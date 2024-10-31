using System;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace ET.Client
{
    [Flags]
    public enum ItemTagType
    {
        Frame = 1,
        Icon = 1 << 2,
        Name = 1 << 3,
        Count = 1 << 4,
    }

    public partial class ESItem
    {
        public EntityRef<ItemData> itemData;

        public Dictionary<ItemTagType, Pair<UObject, GameObject>> instDict = new(3);
        public ItemTagType tagType = ItemTagType.Frame | ItemTagType.Icon;
        public ItemMonoView itemView;
    }
}