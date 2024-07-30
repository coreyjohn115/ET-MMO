using UnityEngine;
using UObject = UnityEngine.Object;

namespace ET.Client

{
    [EntitySystemOf(typeof (ESItem))]
    [FriendOf(typeof (ESItem))]
    public static partial class ESItemSystem
    {
        [EntitySystem]
        private static void Awake(this ESItem self, Transform transform)
        {
            self.uiTransform = transform;
            self.itemView = transform.GetComponent<ItemMonoView>();
            self.E_ContentButton.AddListenerAsync(self.ItemClick);
        }

        [EntitySystem]
        private static void Destroy(this ESItem self)
        {
            self.itemData = default;
            self.DestroyWidget();
            foreach (var pair in self.insts.Values)
            {
                GameObjectPoolHelper.ReturnObjectToPool(pair.Value);
            }

            self.insts.Clear();
        }

        /// <summary>
        /// 设置道具数量显示
        /// </summary>
        /// <param name="self"></param>
        public static void RefreshCount(this ESItem self)
        {
            if ((self.tagType & ItemTagType.Count) == 0)
            {
                self.tagType |= ItemTagType.Count;
            }

            if (!self.insts.TryGetValue(ItemTagType.Count, out var pair))
            {
                GameObject go = GameObjectPoolHelper.GetObjectFromPool(UIHelper.GetItemPoolName(ItemTagType.Count));
                go.transform.SetParent(self.itemView.Content, false);
                pair = new Pair<UObject, GameObject>() { Key = go.GetComponent<ExtendText>(), Value = go };
                self.insts.Add(ItemTagType.Count, pair);
            }

            ItemData item = self.itemData;
            ExtendText text = pair.Key as ExtendText;
            text.text = AmountHelper.GetAmountText(item.Count, out Color color);
            text.color = color;
        }

        public static void SetItemData(this ESItem self, ItemData item)
        {
            self.itemData = item;
        }

        public static async ETTask RefreShIcon(this ESItem self)
        {
            if ((self.tagType & ItemTagType.Icon) == 0)
            {
                self.tagType |= ItemTagType.Icon;
            }

            if (!self.insts.TryGetValue(ItemTagType.Icon, out var pair))
            {
                GameObject go = GameObjectPoolHelper.GetObjectFromPool(UIHelper.GetItemPoolName(ItemTagType.Icon));
                go.transform.SetParent(self.itemView.Content, false);
                pair = new Pair<UObject, GameObject>() { Key = go.GetComponent<ExtendImage>(), Value = go };
                self.insts.Add(ItemTagType.Icon, pair);
            }

            ItemData item = self.itemData;
            ExtendImage img = pair.Key as ExtendImage;
            if (img.sprite && img.sprite.name == item.Config.Icon)
            {
                return;
            }

            img.sprite = await self.LoadIconSpriteAsync(item.Config.Icon);
        }

        public static async ETTask RefreshFrame(this ESItem self)
        {
            if ((self.tagType & ItemTagType.Frame) == 0)
            {
                self.tagType |= ItemTagType.Frame;
            }

            if (!self.insts.TryGetValue(ItemTagType.Frame, out var pair))
            {
                GameObject go = GameObjectPoolHelper.GetObjectFromPool(UIHelper.GetItemPoolName(ItemTagType.Frame));
                go.transform.SetParent(self.itemView.Content, false);
                pair = new Pair<UObject, GameObject>() { Key = go.GetComponent<ExtendImage>(), Value = go };
                self.insts.Add(ItemTagType.Frame, pair);
            }

            ItemData item = self.itemData;
            QualityConfig qualityCfg = QualityConfigCategory.Instance.Get(item.Config.Quality);
            ExtendImage img = pair.Key as ExtendImage;
            if (img.sprite && img.sprite.name == qualityCfg.ItemFrame)
            {
                return;
            }

            img.sprite = await self.LoadWidgetSpriteAsync(qualityCfg.ItemFrame);
        }

        public static void RefreshName(this ESItem self)
        {
            if ((self.tagType & ItemTagType.Name) == 0)
            {
                self.tagType |= ItemTagType.Name;
            }

            if (!self.insts.TryGetValue(ItemTagType.Name, out var pair))
            {
                GameObject go = GameObjectPoolHelper.GetObjectFromPool(UIHelper.GetItemPoolName(ItemTagType.Name));
                go.transform.SetParent(self.itemView.Content, false);
                pair = new Pair<UObject, GameObject>() { Key = go.GetComponent<ExtendImage>(), Value = go };
                self.insts.Add(ItemTagType.Name, pair);
            }

            ItemData item = self.itemData;
            QualityConfig qualityCfg = QualityConfigCategory.Instance.Get(item.Config.Quality);
            Language nameCfg = LanguageCategory.Instance.Get(item.Config.Name);
            ExtendText text = pair.Key as ExtendText;
            text.text = nameCfg.Msg;
            text.color = qualityCfg.ColorBytes.BytesColor();
        }

        private static async ETTask ItemClick(this ESItem self)
        {
            await UIComponentHelper.PopItemTips(self, self.itemData);
        }
    }
}