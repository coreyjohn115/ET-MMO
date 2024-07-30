using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof (Scroll_Item_BagMenu))]
    [FriendOf(typeof (Scroll_Item_BagMenu))]
    public static partial class Scroll_Item_BagMenuSystem
    {
        [EntitySystem]
        private static void Awake(this Scroll_Item_BagMenu self)
        {
            
        }

        [EntitySystem]
        private static void Destroy(this Scroll_Item_BagMenu self)
        {
            self.DestroyWidget();
        }

        public static void Refresh(this Scroll_Item_BagMenu self, BagMenuConfig config)
        {
            self.E_NameExtendText.SetText(config.Name);
            ES_ComBag comBag = self.GetParent<ES_ComBag>();
            self.E_SelectExtendImage.SetActive(comBag.SelectMenuId == self.DataId);
        }

        public static Scroll_Item_BagMenu BindTrans(this Scroll_Item_BagMenu self, Transform trans)
        {
            self.uiTransform = trans;
            return self;
        }
    }
}