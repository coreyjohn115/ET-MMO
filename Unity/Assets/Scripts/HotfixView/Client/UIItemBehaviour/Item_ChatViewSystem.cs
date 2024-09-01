using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof (Scroll_Item_Chat))]
    [FriendOf(typeof (Scroll_Item_Chat))]
    public static partial class Scroll_Item_ChatSystem
    {
        [EntitySystem]
        private static void Awake(this Scroll_Item_Chat self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Scroll_Item_Chat self)
        {
            self.DestroyWidget();
        }

        public static Scroll_Item_Chat BindTrans(this Scroll_Item_Chat self, Transform trans)
        {
            self.uiTransform = trans;
            self.collector = trans.GetComponent<ReferenceCollector>();
            self.element = trans.GetComponent<LayoutElement>();
            for (int i = 0; i < trans.childCount; i++)
            {
                GameObjectPoolHelper.ReturnTransformToPool(trans.GetChild(i));
            }

            return self;
        }

        private static void SetMsgType(this Scroll_Item_Chat self, ClientChatUnit unit)
        {
            if (unit.RoleInfo.Id == ConstValue.ChatWorId)
            {
                self.msgType = ChatMsgType.System;
                return;
            }

            if (self.Data.Emjo != 0)
            {
                self.msgType = ChatMsgType.Image;
                return;
            }

            // if (self.Data.ItemList.Count > 0)
            // {
            //     self.msgType = ChatMsgType.Item;
            //     return;
            // }

            self.msgType = ChatMsgType.Text;
        }

        public static void Refresh(this Scroll_Item_Chat self, ClientChatUnit unit)
        {
            self.chatUnit = unit;
            self.Data = unit.Data;
            self.SetMsgType(unit);
            UIChatPrefabType t = UIChatPrefabType.None;
            if (self.msgType == ChatMsgType.Time)
            {
                t = UIChatPrefabType.Time;
            }
            else if (self.msgType == ChatMsgType.System)
            {
                t = UIChatPrefabType.System;
            }
            else
            {
                long playerId = self.Scene().GetComponent<ClientPlayerComponent>().MyId;
                bool isMe = playerId == unit.RoleInfo.Id;
                switch (self.msgType)
                {
                    case ChatMsgType.Text:
                        t = isMe? UIChatPrefabType.SelfText : UIChatPrefabType.OtherText;
                        break;
                    case ChatMsgType.Image:
                        t = isMe? UIChatPrefabType.SelfImage : UIChatPrefabType.OtherImage;
                        break;
                }
            }

            if (t == UIChatPrefabType.None)
            {
                Log.Error($"不存在的聊天预制件类型: {unit.Message}");
                return;
            }

            self.Item?.Dispose();
            self.item = null;

            string prefabName = t.ToString();
            var prefab = self.collector.Get<GameObject>(prefabName);
            GameObjectPoolHelper.InitPool(prefabName, prefab, 5);
            GameObject go = GameObjectPoolHelper.GetObjectFromPool(prefabName);
            go.transform.SetParent(self.uiTransform);
            (go.transform as RectTransform).Normalize();

            Entity item = null;
            Vector2 size = new Vector2();
            switch (t)
            {
                case UIChatPrefabType.System:
                    break;
                case UIChatPrefabType.SelfText:
                {
                    ES_SelfText it = self.AddChild<ES_SelfText, Transform>(go.transform, true);
                    size = it.Refresh(self);
                    item = it;
                    break;
                }
                case UIChatPrefabType.SelfImage:
                {
                    ES_SelfImage it = self.AddChild<ES_SelfImage, Transform>(go.transform, true);
                    size = it.Refresh(self);
                    item = it;
                    break;
                }
                case UIChatPrefabType.OtherText:
                {
                    ES_OtherText it = self.AddChild<ES_OtherText, Transform>(go.transform, true);
                    size = it.Refresh(self);
                    item = it;
                    break;
                }
                case UIChatPrefabType.OtherImage:
                {
                    ES_OtherImage it = self.AddChild<ES_OtherImage, Transform>(go.transform, true);
                    size = it.Refresh(self);
                    item = it;
                    break;
                }
                case UIChatPrefabType.Time:
                {
                    ES_Time it = self.AddChild<ES_Time, Transform>(go.transform, true);
                    size = it.Refresh(self);
                    item = it;
                    break;
                }
            }

            self.element.preferredWidth = size.x;
            self.element.preferredHeight = size.y;
            self.item = item;
        }
    }
}