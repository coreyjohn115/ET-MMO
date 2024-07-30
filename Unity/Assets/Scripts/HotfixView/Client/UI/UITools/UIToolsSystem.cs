using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (UITools))]
    public static partial class UIToolsSystem
    {
        public static void RegisterUIEvent(this UITools self)
        {
            self.ReloadUIEvent();
        }

        public static void ReloadUIEvent(this UITools self)
        {
            self.View.E_GmButton.AddListener(self.OnGmClick);
        }

        private static void OnGmClick(this UITools self)
        {
            self.Scene().GetComponent<UIComponent>().ShowWindowAsync(WindowID.Win_UIGm).NoContext();
        }

        public static void ShowWindow(this UITools self, Entity contextData = null)
        {
        }
    }
}