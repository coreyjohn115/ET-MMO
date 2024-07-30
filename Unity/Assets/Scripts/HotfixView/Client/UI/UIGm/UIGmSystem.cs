using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (UIGm))]
    public static partial class UIGmSystem
    {
        public static void RegisterUIEvent(this UIGm self)
        {
            self.ReloadUIEvent();
        }

        public static void ReloadUIEvent(this UIGm self)
        {
            self.View.E_MenuLoopVerticalScrollRect.AddItemRefreshListener(self.MenuRefresh);
            self.View.E_SubMenuLoopVerticalScrollRect.AddItemRefreshListener(self.SubMenuRefresh);
            self.View.E_ClickButton.AddListener(self.SendClick);
        }

        private static void MenuRefresh(this UIGm self, Transform transform, int i)
        {
            Scroll_Item_Gm item = self.menuDict[i].BindTrans(transform);
            item.DataId = i;
            item.E_BtnButton.AddListener(() => { self.MenuClick(i); });
            GmCmd cmd = GmCmdCategory.Instance.GetMainList()[i];
            item.E_TextExtendText.text = cmd.Group;
        }

        private static void MenuClick(this UIGm self, int i)
        {
            self.mainSelect = i;
            foreach (KeyValuePair<int, Scroll_Item_Gm> gm in self.menuDict)
            {
                gm.Value.EG_SelectRectTransform.SetActive(gm.Key == i);
            }

            int count = GmCmdCategory.Instance.GetMainList()[i].SubList.Count;
            self.AddUIScrollItems(self.subMenuDict, count);
            self.View.E_SubMenuLoopVerticalScrollRect.SetVisible(true, count);
            self.SubMenuClick(self.subSelect);
        }

        private static void SubMenuRefresh(this UIGm self, Transform transform, int i)
        {
            Scroll_Item_Gm item = self.subMenuDict[i].BindTrans(transform);
            item.DataId = i;
            item.E_BtnButton.AddListener(() => { self.SubMenuClick(i); });
            GmCmd cmd = GmCmdCategory.Instance.GetSubGm(self.mainSelect, i);
            item.E_TextExtendText.text = cmd.Name;
        }

        private static void SubMenuClick(this UIGm self, int i)
        {
            self.subSelect = i;
            foreach (KeyValuePair<int, Scroll_Item_Gm> gm in self.subMenuDict)
            {
                gm.Value.EG_SelectRectTransform.SetActive(gm.Key == i);
            }
            
            GmCmd cmd = GmCmdCategory.Instance.GetSubGm(self.mainSelect, i);
            self.View.E_DescExtendText.text = cmd.Desc;
            self.View.E_InputInputField.text = cmd.Args;
        }

        public static void ShowWindow(this UIGm self, Entity contextData = null)
        {
            self.AddUIScrollItems(self.menuDict, GmCmdCategory.Instance.GetMainList().Count);
            self.View.E_MenuLoopVerticalScrollRect.SetVisible(true, self.menuDict.Count);

            self.MenuClick(self.mainSelect);
        }

        private static void SendClick(this UIGm self)
        {
            GmCmd cmd = GmCmdCategory.Instance.GetSubGm(self.mainSelect, self.subSelect);
            C2M_GMRequest request = C2M_GMRequest.Create();
            request.Cmd = cmd.Cmd;
            request.Args = cmd.ArgList;
            self.Root().GetComponent<ClientSenderComponent>()?.Send(request);
        }

        public static void BeforeUnload(this UIGm self)
        {
            self.View.E_MenuLoopVerticalScrollRect.ReturnPool();
            self.View.E_SubMenuLoopVerticalScrollRect.ReturnPool();
        }
    }
}