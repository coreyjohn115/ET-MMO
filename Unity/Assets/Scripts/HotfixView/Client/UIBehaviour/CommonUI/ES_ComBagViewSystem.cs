using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof (ES_ComBag))]
    [FriendOf(typeof (ES_ComBag))]
    public static partial class ES_ComBagSystem
    {
        [UICom(nameof (ES_ComBag))]
        private class ComBagShow: AUIComHandler
        {
            public override void Show(Entity uiCom)
            {
                uiCom.GetParent<ES_ComBag>().Show();
            }
        }
        
        [EntitySystem]
        private static void Awake(this ES_ComBag self, Transform transform)
        {
            self.uiTransform = transform;
            if (self.menuList.Count == 0)
            {
                //全部
                self.menuList.Add(new BagMenuConfig() { Id = 0, Name = 200010, ItemTypes = new HashSet<ItemType>() });
                //装备
                self.menuList.Add(new BagMenuConfig() { Id = 1, Name = 200011, ItemTypes = new HashSet<ItemType>() { ItemType.Equip } });
                //材料
                self.menuList.Add(new BagMenuConfig() { Id = 2, Name = 200012, ItemTypes = new HashSet<ItemType>() { ItemType.Normal } });
                //消耗
                self.menuList.Add(new BagMenuConfig() { Id = 3, Name = 200013, ItemTypes = new HashSet<ItemType>() { ItemType.Pet } });
            }

            self.comparer = new ItemComparer();
            self.E_MenuListLoopHorizontalScrollRect.AddItemRefreshListener(self.MenuRefresh);
            self.AddUIScrollItems(self.bagMenuDict, self.menuList.Count);

            self.E_ItemGridLoopVerticalScrollRect.AddItemRefreshListener(self.ItemRefresh);
        }

        [EntitySystem]
        private static void Destroy(this ES_ComBag self)
        {
            self.E_MenuListLoopHorizontalScrollRect.ReturnPool();
            self.E_ItemGridLoopVerticalScrollRect.ReturnPool();
            self.DestroyWidget();
        }

        private static void Show(this ES_ComBag self)
        {
            self.E_MenuListLoopHorizontalScrollRect.SetVisible(true, self.menuList.Count);
            self.RefreshItemList();
        }

        private static void RefreshItemList(this ES_ComBag self)
        {
            self.itemList.Clear();
            var list = self.Root().GetComponent<ClientItemComponent>().Children.Values;
            var filterList = self.menuList[self.SelectMenuId].ItemTypes;
            foreach (ItemData item in list)
            {
                if (filterList.Count == 0 || filterList.Contains(item.ItemType))
                {
                    self.itemList.Add(item);
                }
            }

            self.itemList.Sort(self.comparer);
            self.AddUIScrollItems(self.bagDict, self.itemList.Count);
            self.E_ItemGridLoopVerticalScrollRect.SetVisible(true, self.itemList.Count);
        }

        private static void MenuRefresh(this ES_ComBag self, Transform transform, int idx)
        {
            Scroll_Item_BagMenu item = self.bagMenuDict[idx].BindTrans(transform);
            item.DataId = idx;
            BagMenuConfig menuCfg = self.menuList[idx];

            item.Refresh(menuCfg);
            item.E_BtnButton.AddListener(() => { self.MenuClick(menuCfg); });
        }

        private static void MenuClick(this ES_ComBag self, BagMenuConfig config)
        {
            self.SelectMenuId = config.Id;
            foreach (var menu in self.bagMenuDict)
            {
                BagMenuConfig menuCfg = self.menuList[menu.Key];
                menu.Value.Refresh(menuCfg);
            }
            
            self.RefreshItemList();
        }

        private static void ItemRefresh(this ES_ComBag self, Transform transform, int idx)
        {
            Scroll_Item_Bag bag = self.bagDict[idx].BindTrans(transform);
            bag.DataId = idx;
            bag.SetCacheMode(true);

            ItemData item = self.itemList[idx];
            bag.Refresh(item).NoContext();
        }
    }
}