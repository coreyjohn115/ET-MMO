using UnityEngine;
using UnityEngine.UI;

namespace ET.Client

{
    [EntitySystemOf(typeof (ES_SelfText))]
    [FriendOf(typeof (ES_SelfText))]
    public static partial class ES_SelfTextSystem
    {
        [EntitySystem]
        private static void Awake(this ES_SelfText self, Transform transform)
        {
            self.uiTransform = transform;
        }

        [EntitySystem]
        private static void Destroy(this ES_SelfText self)
        {
            self.DestroyWidget();
        }

        public static Vector2 Refresh(this ES_SelfText self, Scroll_Item_Chat item)
        {
            self.E_MsgSymbolText.text = item.Data.Msg;
            self.E_MsgLongClickButton.OnLongClick.AddListener(self.OnLongClick);

            float width = self.E_MsgSymbolText.preferredWidth;
            if (width > ClientChatComponent.InitWidth)
            {
                self.E_MsgSymbolText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ClientChatComponent.InitWidth);
                self.E_BgExtendImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ClientChatComponent.InitWidth + ES_SelfText.xOffset);
            }
            else
            {
                self.E_MsgSymbolText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                self.E_BgExtendImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + ES_SelfText.xOffset);
            }

            float height = self.E_MsgSymbolText.preferredHeight;
            self.E_MsgSymbolText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            self.E_BgExtendImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + ES_SelfText.yOffset);

            return self.GetSize();
        }

        private static Vector2 GetSize(this ES_SelfText self)
        {
            RectTransform imageTrans = self.E_BgExtendImage.rectTransform;
            float x = (self.uiTransform as RectTransform).sizeDelta.x;
            return new Vector2(x, imageTrans.rect.height + Mathf.Abs(imageTrans.anchoredPosition.y) + ClientChatComponent.Gap);
        }

        private static void OnLongClick(this ES_SelfText self, bool isOver)
        {
            Log.Info("OnLongClick");
        }
    }
}