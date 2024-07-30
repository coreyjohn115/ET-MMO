using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof (Scroll_Item_Bag))]
    [FriendOf(typeof (Scroll_Item_Bag))]
    public static partial class Scroll_Item_BagSystem
    {
        [EntitySystem]
        private static void Awake(this Scroll_Item_Bag self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Scroll_Item_Bag self)
        {
            self.DestroyWidget();
        }

        public static async ETTask Refresh(this Scroll_Item_Bag self, ItemData item)
        {
            self.ESItem.SetItemData(item);
            await self.ESItem.RefreshFrame();
            await self.ESItem.RefreShIcon();
            self.ESItem.RefreshCount();
        }

        public static Scroll_Item_Bag BindTrans(this Scroll_Item_Bag self, Transform trans)
        {
            self.uiTransform = trans;
            return self;
        }
    }
}