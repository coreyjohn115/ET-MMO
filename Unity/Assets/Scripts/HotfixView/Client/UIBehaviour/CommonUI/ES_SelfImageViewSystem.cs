using UnityEngine;
using UnityEngine.UI;

namespace ET.Client

{
    [EntitySystemOf(typeof (ES_SelfImage))]
    [FriendOf(typeof (ES_SelfImage))]
    public static partial class ES_SelfImageSystem
    {
        [EntitySystem]
        private static void Awake(this ES_SelfImage self, Transform transform)
        {
            self.uiTransform = transform;
        }

        [EntitySystem]
        private static void Destroy(this ES_SelfImage self)
        {
            self.DestroyWidget();
        }

        public static Vector2 Refresh(this ES_SelfImage self, Scroll_Item_Chat item)
        {
            EmojiConfig config = EmojiConfigCategory.Instance.Get(item.Data.Emjo);
            self.SetSprite(self.E_IconExtendImage, config.Icon).NoContext();
            RectTransform trans = self.E_IconExtendImage.rectTransform;
            return new Vector2(trans.sizeDelta.x, trans.rect.height + Mathf.Abs(trans.anchoredPosition.y) + ClientChatComponent.Gap);
        }
    }
}