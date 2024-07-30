using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof (Scroll_Item_Menu))]
    [FriendOf(typeof (Scroll_Item_Menu))]
    public static partial class Scroll_Item_MenuSystem
    {
        [EntitySystem]
        private static void Awake(this Scroll_Item_Menu self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Scroll_Item_Menu self)
        {
            self.DestroyWidget();
        }

        public static Scroll_Item_Menu BindTrans(this Scroll_Item_Menu self, Transform trans)
        {
            self.uiTransform = trans;
            return self;
        }

        public static void Refresh(this Scroll_Item_Menu self, MeunData data, int selectId)
        {
            self.meunData = data;
            bool select = self.DataId == selectId;
            self.EG_SelectRectTransform.SetActive(select);
            if (self.E_TextExtendText != null)
            {
                self.E_TextExtendText.SetText(data.Config.Name);
            }

            if (self.EG_LockRectTransform != null)
            {
                self.EG_LockRectTransform.SetActive(false);
            }

            if (self.E_IconExtendImage != null)
            {
                IconHelper.SetSprite(self, self.E_IconExtendImage, data.Config.Icon, AtlasType.Widget).NoContext();
            }

            if (select)
            {
                EventSystem.Instance.Publish(self.Scene(), new MenuSelectEvent() { ItemMenu = self, Data = data, Index = selectId, });
            }
        }

        public static void RefreshAll(this Scroll_Item_Menu self, int i, int menuClassify)
        {
            MenuDict menu = self.Parent.GetChild<MenuDict>(menuClassify);
            foreach ((int o, Scroll_Item_Menu item) in menu.MenuDic)
            {
                if (o == i)
                {
                    continue;
                }

                bool select = item.DataId == menu.SelectId;
                item.EG_SelectRectTransform.SetActive(select);
            }
        }
    }
}