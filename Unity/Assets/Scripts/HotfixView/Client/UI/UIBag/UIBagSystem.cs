using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (UIBag))]
    public static partial class UIBagSystem
    {
        public static void RegisterUIEvent(this UIBag self)
        {
            self.ReloadUIEvent();
        }

        public static void ReloadUIEvent(this UIBag self)
        {
            self.View.E_BagMenuListLoopVerticalScrollRect.AddMenuRefreshListener(self, SystemMenuType.Bag);
        }

        public static void ShowWindow(this UIBag self, Entity contextData = null)
        {
            self.View.E_BagMenuListLoopVerticalScrollRect.SetMenuVisible(self, SystemMenuType.Bag);
        }
    }
}