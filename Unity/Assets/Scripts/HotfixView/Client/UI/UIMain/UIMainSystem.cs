using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (UIMain))]
    public static partial class UIMainSystem
    {
        public static void RegisterUIEvent(this UIMain self)
        {
            self.ReloadUIEvent();
        }

        public static void ReloadUIEvent(this UIMain self)
        {
            self.View.E_ChatButton.AddListenerAsync(self.ChatBtnClick);
            self.View.E_BottomMenuListLoopVerticalScrollRect.AddMenuRefreshListener(self, SystemMenuType.BottomMenu, MenuSelectMode.None);
        }

        public static void ShowWindow(this UIMain self, Entity contextData = null)
        {
            self.View.E_BottomMenuListLoopVerticalScrollRect.SetMenuVisible(self, SystemMenuType.BottomMenu);
        }

        private static async ETTask ChatBtnClick(this UIMain self)
        {
            await self.Scene().GetComponent<UIComponent>().ShowWindow<UIChat>();
        }
    }
}