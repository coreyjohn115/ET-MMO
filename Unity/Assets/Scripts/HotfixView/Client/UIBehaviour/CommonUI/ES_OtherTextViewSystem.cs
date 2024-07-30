using UnityEngine;
using UnityEngine.UI;

namespace ET.Client

{
    [EntitySystemOf(typeof (ES_OtherText))]
    [FriendOf(typeof (ES_OtherText))]
    public static partial class ES_OtherTextSystem
    {
        [EntitySystem]
        private static void Awake(this ES_OtherText self, Transform transform)
        {
            self.uiTransform = transform;
        }

        [EntitySystem]
        private static void Destroy(this ES_OtherText self)
        {
            self.DestroyWidget();
        }

        public static Vector2 Refresh(this ES_OtherText self, Scroll_Item_Chat item)
        {
            self.E_MsgSymbolText.text = item.Data.Msg;
            self.E_MsgLongClickButton.OnLongClick.AddListener(self.OnLongClick);

            float width = self.E_MsgSymbolText.preferredWidth;
            if (width > ClientChatComponent.InitWidth)
            {
                self.E_MsgSymbolText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ClientChatComponent.InitWidth);
                self.E_BgExtendImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ClientChatComponent.InitWidth + ES_OtherText.xOffset);
            }
            else
            {
                self.E_MsgSymbolText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                self.E_BgExtendImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + ES_OtherText.xOffset);
            }

            float height = self.E_MsgSymbolText.preferredHeight;
            self.E_MsgSymbolText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            self.E_BgExtendImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + ES_OtherText.yOffset);

            return self.GetSize();
        }

        private static Vector2 GetSize(this ES_OtherText self)
        {
            RectTransform imageTrans = self.E_BgExtendImage.rectTransform;
            float x = (self.uiTransform as RectTransform).sizeDelta.x;
            return new Vector2(x, imageTrans.rect.height + Mathf.Abs(imageTrans.anchoredPosition.y) + ClientChatComponent.Gap);
        }

        private static void OnLongClick(this ES_OtherText self, bool isOver)
        {
        }
    }
}