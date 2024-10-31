using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof (Scroll_Item_Emoj))]
    [FriendOf(typeof (Scroll_Item_Emoj))]
    public static partial class Scroll_Item_EmojSystem
    {
        [EntitySystem]
        private static void Awake(this Scroll_Item_Emoj self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Scroll_Item_Emoj self)
        {
            self.DestroyWidget();
        }

        public static Scroll_Item_Emoj BindTrans(this Scroll_Item_Emoj self, Transform trans)
        {
            self.uiTransform = trans;
            return self;
        }

        public static void Refresh(this Scroll_Item_Emoj self, int id)
        {
            var cfg = EmojiConfigCategory.Instance.Get(id);
            self.SetSprite(self.E_IconExtendImage, cfg.Icon).NoContext();
        }
    }
}