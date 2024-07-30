using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (UIConfirm))]
    public static partial class UIConfirmSystem
    {
        public static void RegisterUIEvent(this UIConfirm self)
        {
            self.ReloadUIEvent();

            self.btnGoList.Add(self.View.E_OkButton.gameObject);
            self.btnGoList.Add(self.View.E_CancelButton.gameObject);
        }

        public static void ReloadUIEvent(this UIConfirm self)
        {
            self.View.E_CloseButton.AddListener(self.OnCloseClick);
            self.View.E_EmptyCloseButton.AddListener(self.OnEmptyClick);

            self.View.E_OkButton.AddListener(self.OnOkClick);
            self.View.E_CancelButton.AddListener(self.OnCancelClick);
        }

        public static void ShowWindow(this UIConfirm self, Entity showData = null)
        {
            UIConfirmData data = showData as UIConfirmData;

            self.View.E_TitleExtendText.text = data.Title;
            self.View.E_DescExtendText.text = data.Desc;
            self.extra = data.Extra;
            self.View.E_ToggleToggle.isOn = false;
            self.View.E_ToggleToggle.SetActive(self.extra.TipType != default);

            foreach (GameObject go in self.btnGoList)
            {
                go.SetActive(false);
            }

            foreach (ConfirmBtn btn in data.BtnList)
            {
                Language lang = LanguageCategory.Instance.Get2(btn.TextId);
                switch (btn.ConfirmType)
                {
                    case UIConfirmType.Ok:
                        self.View.E_OkButton.SetActive(true);
                        self.View.E_OkButton.GetComponent<StateActiveView>().SetStateActive((int)btn.ConfirmStyle);
                        if (lang != null)
                        {
                            self.View.E_OkTextExtendText.text = lang.Msg;
                        }

                        break;
                    case UIConfirmType.Cancel:
                        self.View.E_CancelButton.SetActive(true);
                        self.View.E_CancelButton.GetComponent<StateActiveView>().SetStateActive((int)btn.ConfirmStyle);
                        if (lang != null)
                        {
                            self.View.E_CancelTextExtendText.text = lang.Msg;
                        }

                        break;
                }
            }
        }

        public static void HideWindow(this UIConfirm self)
        {
            self.extra = default;
            self.Root().GetComponent<ObjectWait>().Notify(new Wait_CloseConfirm() { ConfirmType = self.confirmType });
        }

        /// <summary>
        /// 获取勾选了不再提示的选择类型
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tipType"></param>
        /// <returns></returns>
        public static UIConfirmType GetTipConfirmType(this UIConfirm self, ConfirmTipType tipType)
        {
            return self.nTipDict.GetValueOrDefault(tipType);
        }

        private static void SetNTip(this UIConfirm self)
        {
            if (self.extra.TipType == default)
            {
                return;
            }

            if (self.View.E_ToggleToggle.isActiveAndEnabled && self.View.E_ToggleToggle.isOn)
            {
                self.nTipDict.TryAdd(self.extra.TipType, self.confirmType);
            }
        }

        private static void OnOkClick(this UIConfirm self)
        {
            self.confirmType = UIConfirmType.Ok;
            self.SetNTip();
            self.Root().GetComponent<UIComponent>().HideWindow(WindowID.Win_UIConfirm);
        }

        private static void OnCancelClick(this UIConfirm self)
        {
            self.confirmType = UIConfirmType.Cancel;
            self.SetNTip();
            self.Root().GetComponent<UIComponent>().HideWindow(WindowID.Win_UIConfirm);
        }

        private static void OnCloseClick(this UIConfirm self)
        {
            self.confirmType = UIConfirmType.None;
            self.Root().GetComponent<UIComponent>().HideWindow(WindowID.Win_UIConfirm);
        }

        private static void OnEmptyClick(this UIConfirm self)
        {
            if (self.extra.EmptyClose)
            {
                self.OnCloseClick();
            }
        }
    }
}