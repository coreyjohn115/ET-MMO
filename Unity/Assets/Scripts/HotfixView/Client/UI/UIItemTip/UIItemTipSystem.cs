using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (UIItemTip))]
    public static partial class UIItemTipSystem
    {
        public static void RegisterUIEvent(this UIItemTip self)
        {
            self.ReloadUIEvent();
        }

        public static void ReloadUIEvent(this UIItemTip self)
        {
        }

        private static void SetActive(Entity entity, bool active)
        {
            foreach (Entity child in entity.Children.Values)
            {
                if (child is IUIItemTipCom com)
                {
                    com.SetActive(active);
                }
            }
        }

        private static IUIItemTipCom GetChild(Entity entity, Type t)
        {
            foreach (Entity child in entity.Children.Values)
            {
                if (child.GetType() == t)
                {
                    return child as IUIItemTipCom;
                }
            }

            return default;
        }

        [EnableAccessEntiyChild]
        private static async ETTask CreateTip(Scene scene, string path, string type, Entity entity, Transform parent, ItemData item)
        {
            Type t = CodeTypes.Instance.GetType(type);
            SetActive(entity, false);
            IUIItemTipCom child = GetChild(entity, t);
            if (child != default)
            {
                child.SetActive(true);
            }
            else
            {
                var loader = scene.GetComponent<CurrentScenesComponent>().Scene.GetComponent<ResourcesLoaderComponent>();
                GameObject prefab = await loader.LoadAssetAsync<GameObject>(path);
                GameObject go = UnityEngine.Object.Instantiate(prefab, parent);

                Entity c = entity.AddChildByType(go.transform, t);
                c.AddComponent<UIComComponent>();
                child = c as IUIItemTipCom;
            }

            child.SetItem(item);
            UIComComponent uiCom = entity.GetChildByName(t.Name).GetComponent<UIComComponent>();
            uiCom.Show();
        }

        public static void ShowWindow(this UIItemTip self, Entity contextData = null)
        {
            ItemData item = contextData as ItemData;
            if (!item)
            {
                Log.Error("item tip contextData is null");
                return;
            }

            string comPath = string.Empty;
            string comType = string.Empty;
            switch (item.ItemType)
            {
                case ItemType.Normal:
                    comType = "ET.Client.ES_NormalItem";
                    comPath = $"Assets/Bundles/UI/Window/ItemTip/ES_NormalItem.prefab";
                    break;
                case ItemType.Equip:
                    comType = "ET.Client.ES_EquipItem";
                    comPath = $"Assets/Bundles/UI/Window/ItemTip/ES_EquipItem.prefab";
                    break;
            }

            if (comPath.IsNullOrEmpty())
            {
                Log.Error("item tip comPath is null");
                return;
            }

            CreateTip(self.Root(), comPath, comType, self, self.View.EG_ContentRectTransform, item).NoContext();
        }
    }
}