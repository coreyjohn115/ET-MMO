using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (UIChat))]
    [FriendOf(typeof (ClientChatComponent))]
    public static partial class UIChatSystem
    {
        private static async ETTask LoadData(this UIChat self)
        {
            UIChat data = await self.Scene().GetComponent<DataSaveComponent>().GetAsync<UIChat>("historyEmjiList");
            if (data != null)
            {
                self.historyEmojList.Clear();
                self.historyEmojList.AddRange(data.historyEmojList);
                data.Dispose();

                self.AddUIScrollItems(self.emojHistoryDict, self.historyEmojList.Count);
                self.View.E_HistoryListLoopHorizontalScrollRect.SetVisible(true, self.historyEmojList.Count);
            }
        }

        public static void RegisterUIEvent(this UIChat self)
        {
            self.moveTween = self.View.EG_ChatRectTransform.GetComponent<TweenAnchorPosition>();
            self.moveTween.TweenComplete(self.MoveComplete);
            self.emojiTween = self.View.EG_AnimRectTransform.GetComponent<TweenAnchorPosition>();
            self.View.E_BackBtnButton.AddListener(self.BackBtnClick);
            self.View.E_SettingBtnButton.AddListener(self.SettingBtnClick);
            self.View.E_SendButton.AddListener(self.SendBtnClick);
            self.View.E_EmjoButton.AddListener(self.EmjoBtnClick);
            self.View.E_CloseEmojButton.AddListener(self.CloseEmjoBtnClick);

            self.View.E_InputInputField.onSubmit.AddListener(self.OnSubmit);

            self.View.E_MenuListLoopHorizontalScrollRect.AddMenuRefreshListener(self, SystemMenuType.Chat);
            self.View.E_MsgListLoopVerticalScrollRect.AddItemRefreshListener(self.MsgListRefresh);
            self.View.E_EmotionMeuListLoopHorizontalScrollRect.AddMenuRefreshListener(self, SystemMenuType.ChatEmojMenu);
            self.View.E_HistoryListLoopHorizontalScrollRect.AddItemRefreshListener(self.EmojHistoryRefresh);
            self.View.E_EmojListLoopVerticalScrollRect.AddItemRefreshListener(self.EmojRefresh);

            self.LoadData().NoContext();
        }

        public static void ShowWindow(this UIChat self, Entity contextData = null)
        {
            self.temphistoryEmojList.Clear();
            self.View.E_CloseEmojButton.SetActive(false);
            self.moveTween.PlayForward();
            self.View.E_TitleExtendText.SetTitle(WindowID.Win_UIChat);

            //菜单列表
            self.View.E_MenuListLoopHorizontalScrollRect.SetMenuVisible(self, SystemMenuType.Chat);
            self.View.E_EmotionMeuListLoopHorizontalScrollRect.SetMenuVisible(self, SystemMenuType.ChatEmojMenu);
        }

        public static void BeforeUnload(this UIChat self)
        {
            self.View.E_MenuListLoopHorizontalScrollRect.ClearPool();
            self.View.E_EmotionMeuListLoopHorizontalScrollRect.ClearPool();
            self.View.E_HistoryListLoopHorizontalScrollRect.ClearPool();
            self.View.E_EmojListLoopVerticalScrollRect.ClearPool();
            self.View.E_MsgListLoopVerticalScrollRect.ReturnPool();
        }

        private static void BackBtnClick(this UIChat self)
        {
            self.moveTween.PlayReverse();
        }

        //打开设置界面
        private static void SettingBtnClick(this UIChat self)
        {
        }

        private static void SendInnner(this UIChat self)
        {
            self.data.Msg = ChatHelper.RevertEmojiName(self.data.Msg, EmojiConfigCategory.Instance.GetEmojiName);
            int index = self.GetChild<MenuDict>(SystemMenuType.Chat).SelectId;
            C2Chat_SendRequest chat = C2Chat_SendRequest.Create();

            string msg = ChatHelper.Encode(self.data);
            chat.Message = msg;
            PlayerInfoProto playerInfo = PlayerInfoProto.Create();
            playerInfo.Id = self.Scene().GetComponent<ClientPlayerComponent>().MyId;
            playerInfo.Name = "111111";
            playerInfo.Level = 1;
            playerInfo.Fight = 1L;
            playerInfo.HeadIcon = default;
            chat.RoleInfo = playerInfo;

            switch (index)
            {
                //世界
                case 0:
                    chat.Channel = (int)ChatChannelType.World;
                    chat.GroupId = ConstValue.ChatSendId;
                    break;
                //帮会
                case 1:
                    chat.Channel = (int)ChatChannelType.League;
                    break;
            }

            self.Root().GetComponent<ClientSenderComponent>().Send(chat);
            self.View.E_InputInputField.text = string.Empty;
        }

        private static void SendBtnClick(this UIChat self)
        {
            string msg = self.View.E_InputInputField.text;
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }

            //刷新历史表情
            if (self.temphistoryEmojList.Count > 0)
            {
                self.historyEmojList.InsertRange(0, self.temphistoryEmojList);
                if (self.historyEmojList.Count > 8)
                {
                    self.historyEmojList.RemoveRange(0, self.historyEmojList.Count - 8);
                }

                self.temphistoryEmojList.Clear();
            }

            if (self.GetChild<MenuDict>(SystemMenuType.ChatEmojMenu).SelectId == 0)
            {
                self.AddUIScrollItems(self.emojHistoryDict, self.historyEmojList.Count);
                self.View.E_HistoryListLoopHorizontalScrollRect.SetVisible(true, self.historyEmojList.Count);
            }

            self.data.Msg = self.View.E_InputInputField.text;
            self.SendInnner();
            self.View.E_InputInputField.text = string.Empty;
        }

        private static void EmjoBtnClick(this UIChat self)
        {
            self.emojiTween.PlayForward();
            self.View.E_CloseEmojButton.SetActive(true);
        }

        private static void CloseEmjoBtnClick(this UIChat self)
        {
            self.Scene().GetComponent<DataSaveComponent>().SaveAsync("historyEmjiList", self).NoContext();
            self.temphistoryEmojList.Clear();
            self.View.E_CloseEmojButton.SetActive(false);
            self.emojiTween.PlayReverse();
        }

        private static void OnSubmit(this UIChat self, string value)
        {
            self.SendBtnClick();
        }

        private static void MoveComplete(this UIChat self, Tweener t)
        {
            if (!t.IsForward)
            {
                self.Scene().GetComponent<UIComponent>().HideWindow<UIChat>();
            }
        }

        private static void MsgListRefresh(this UIChat self, Transform transform, int idx)
        {
            int index = self.GetChild<MenuDict>(SystemMenuType.Chat).SelectId;
            List<ClientChatUnit> chatUnitList = self.GetChatUnitList(index);
            Scroll_Item_Chat item = self.msgDic[idx].BindTrans(transform);
            item.DataId = idx;
            ClientChatUnit unit = chatUnitList[idx];
            item.Refresh(unit);
        }

        private static List<ClientChatUnit> GetChatUnitList(this UIChat self, int index)
        {
            List<ClientChatUnit> chatUnitList = null;
            switch (index)
            {
                //世界
                case 0:
                    chatUnitList = self.Scene().GetComponent<ClientChatComponent>().worldMsgList;
                    break;
                //帮会
                case 1:
                    chatUnitList = self.Scene().GetComponent<ClientChatComponent>().leagueMsgList;
                    break;
                case 2:
                    break;
            }

            return chatUnitList;
        }

        public static void AddMsg(this UIChat self, ClientChatUnit unit, bool animate = true)
        {
            int index = self.GetChild<MenuDict>(SystemMenuType.Chat).SelectId;
            List<ClientChatUnit> chatUnitList = self.GetChatUnitList(index);
            if (chatUnitList == null)
            {
                return;
            }

            self.View.E_MsgListLoopVerticalScrollRect.totalCount = chatUnitList.Count;
            Scroll_Item_Chat item = self.AddChild<Scroll_Item_Chat>();
            self.msgDic.Add(chatUnitList.Count - 1, item);
            if (animate)
            {
                self.View.E_MsgListLoopVerticalScrollRect.SrollToCell(chatUnitList.Count - 1, 700);
            }
            else
            {
                self.View.E_MsgListLoopVerticalScrollRect.SetVisible(true, chatUnitList.Count, true);
            }
        }

        public static void RefreshChatList(this UIChat self, int index)
        {
            List<ClientChatUnit> chatUnitList = self.GetChatUnitList(index);
            if (chatUnitList == null)
            {
                return;
            }

            self.AddUIScrollItems(self.msgDic, chatUnitList.Count);
            self.View.E_MsgListLoopVerticalScrollRect.SetVisible(true, chatUnitList.Count, true);
        }

        private static void EmojHistoryRefresh(this UIChat self, Transform transform, int idx)
        {
            Scroll_Item_Emoj item = self.emojHistoryDict[idx].BindTrans(transform);
            item.DataId = idx;
            int id = self.historyEmojList[idx];
            item.Refresh(id);
            item.E_BtnButton.AddListener(() => { self.EmojiItemClick(id); });
        }

        private static void EmojRefresh(this UIChat self, Transform transform, int idx)
        {
            Scroll_Item_Emoj item = self.emojDict[idx].BindTrans(transform);
            item.DataId = idx;
            int group = self.GetChild<MenuDict>(SystemMenuType.ChatEmojMenu).GetGroupId();
            var list = EmojiConfigCategory.Instance.GetGroupList(group);
            int id = list[idx].Id;
            item.Refresh(id);
            item.E_BtnButton.AddListener(() => { self.EmojiItemClick(id); });
        }

        private static void EmojiItemClick(this UIChat self, int id)
        {
            if (self.GetChild<MenuDict>(SystemMenuType.ChatEmojMenu).SelectId == 0)
            {
                self.data.Emjo = 0;
                var cfg = EmojiConfigCategory.Instance.Get(id);
                var l = LanguageCategory.Instance.Get(cfg.Name);
                self.View.E_InputInputField.text = $"{self.View.E_InputInputField.text}[{l.Msg}]";
                if (!self.historyEmojList.Contains(id))
                {
                    self.temphistoryEmojList.Remove(id);
                    self.temphistoryEmojList.Insert(0, id);
                }
                else
                {
                    self.historyEmojList.Remove(id);
                    self.historyEmojList.Insert(0, id);
                }
            }
            else
            {
                self.data.Emjo = id;
                self.SendInnner();
            }
        }

        public static void RefreshEmojiList(this UIChat self, MenuSelectEvent a)
        {
            //第一页才有历史记录
            self.View.E_HistoryListLoopHorizontalScrollRect.SetActive(a.Index == 0);
            if (a.Index == 0)
            {
                if (self.historyEmojList.Count == 0)
                {
                    self.View.E_HistoryListLoopHorizontalScrollRect.SetActive(false);
                }
                else
                {
                    self.AddUIScrollItems(self.emojHistoryDict, self.historyEmojList.Count);
                    self.View.E_HistoryListLoopHorizontalScrollRect.SetVisible(true, self.historyEmojList.Count);
                }
            }

            var list = EmojiConfigCategory.Instance.GetGroupList(a.Data.Config.GroupId);
            self.AddUIScrollItems(self.emojDict, list.Count);
            self.View.E_EmojListLoopVerticalScrollRect.SetVisible(true, list.Count);
        }
    }
}