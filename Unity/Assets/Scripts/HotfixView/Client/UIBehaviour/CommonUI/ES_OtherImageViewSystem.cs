using UnityEngine;
using UnityEngine.UI;

namespace ET.Client

{
    [EntitySystemOf(typeof (ES_OtherImage))]
    [FriendOf(typeof (ES_OtherImage))]
    public static partial class ES_OtherImageSystem
    {
        [EntitySystem]
        private static void Awake(this ES_OtherImage self, Transform transform)
        {
            self.uiTransform = transform;
        }

        [EntitySystem]
        private static void Destroy(this ES_OtherImage self)
        {
            self.DestroyWidget();
        }

        public static Vector2 Refresh(this ES_OtherImage self, Scroll_Item_Chat item)
        {
            EmojiConfig config = EmojiConfigCategory.Instance.Get(item.Data.Emjo);
            IconHelper.SetSprite(self, self.E_IconExtendImage, config.Icon, AtlasType.Emotion).NoContext();
            RectTransform trans = self.E_IconExtendImage.rectTransform;
            return new Vector2(trans.sizeDelta.x, trans.rect.height + Mathf.Abs(trans.anchoredPosition.y) + ClientChatComponent.Gap);
        }
    }
}