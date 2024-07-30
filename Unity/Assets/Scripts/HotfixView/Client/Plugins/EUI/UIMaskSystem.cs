using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (UIMask))]
    [EntitySystemOf(typeof (UIMask))]
    public static partial class UIMaskSystem
    {
        [EntitySystem]
        private static void Awake(this UIMask self, bool clickClose)
        {
            self.ClickHide = clickClose;
            GameObject Go = new("Mask", typeof (Image));
            Go.layer = LayerNames.GetLayerInt(LayerNames.UI);
            Image mask = Go.GetComponent<Image>();
            mask.color = new Color(0, 0, 0, 0.8f);
            self.Mask = mask.rectTransform;
            self.Mask.SetParent(Global.Instance.UI, false);
            self.Mask.SetActive(false);
        }

        [EntitySystem]
        private static void Destroy(this UIMask self)
        {
            if (self.Mask == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(self.Mask);
            self.Mask = null;
        }

        public static void SetActive(this UIMask self, Transform parent)
        {
            self.Mask.Normalize();
            self.Mask.SetParent(parent, false);
            self.Mask.SetAsLastSibling();
            self.Mask.SetActive(true);
        }

        public static void SetSibling(this UIMask self, Transform compare)
        {
            var index = compare.GetSiblingIndex();
            self.Mask.SetParent(compare.parent, false);
            self.Mask.SetSiblingIndex(index);
        }

        public static void Hide(this UIMask self)
        {
            self.Mask.SetAsFirstSibling();
            self.Mask.SetActive(false);
        }
    }
}