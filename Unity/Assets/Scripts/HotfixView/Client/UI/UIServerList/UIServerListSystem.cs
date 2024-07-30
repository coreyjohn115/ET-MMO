using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (ServerInfoComponent))]
    [FriendOf(typeof (UIServerList))]
    public static partial class UIServerListSystem
    {
        public static void RegisterUIEvent(this UIServerList self)
        {
            self.View.E_ServerListLoopVerticalScrollRect.AddItemRefreshListener(self.OnScrollItemRefreshHandler);
        }

        public static void ShowWindow(this UIServerList self, Entity contextData = null)
        {
            var serverCom = self.Scene().GetComponent<ServerInfoComponent>();
            int count = serverCom.ServerInfoList.Count;
            self.AddUIScrollItems(self.ItemServerDict, count);
            self.View.E_ServerListLoopVerticalScrollRect.SetVisible(true, count);
        }

        public static void BeforeUnload(this UIServerList self)
        {
            self.View.E_ServerListLoopVerticalScrollRect.ClearPool();
        }

        private static void OnScrollItemRefreshHandler(this UIServerList self, Transform transform, int index)
        {
            var serverTest = self.ItemServerDict[index].BindTrans(transform);
            ServerInfo info = self.Scene().GetComponent<ServerInfoComponent>().ServerInfoList[index];
            serverTest.E_NameExtendText.SetText(info.ServerName);
            serverTest.E_ServerButton.AddListener(() => { self.OnSelectServerItemHandler(info.Id); });
        }

        private static void OnSelectServerItemHandler(this UIServerList self, long serverId)
        {
            self.Scene().GetComponent<ServerInfoComponent>().CurrentServerId = (int) serverId;
            Log.Debug($"当前选择的服务器 Id 是:{serverId}");
            self.Scene().GetComponent<UIComponent>().HideWindow(WindowID.Win_UIServerList);
        }
    }
}