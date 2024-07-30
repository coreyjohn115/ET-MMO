using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// UI组件方法扩展
    /// </summary>
    public static class UIComponentHelper
    {
        public static void PopMsg(Entity entity, string msg, Color? color = default)
        {
            entity.Root().GetComponent<UIComponent>().GetDlgLogic<UIPop>().PopMsg(msg, color);
        }

        public static async ETTask PopItemTips(Entity self, ItemData itemData)
        {
            await self.Root().GetComponent<UIComponent>().ShowWindowAsync(WindowID.Win_UIItemTip, itemData);
        }

        public static async ETTask CheckCloseUI(Entity self)
        {
            bool hide = self.Root().GetComponent<UIComponent>().HideLast();
            if (hide)
            {
                return;
            }

            UIConfirmType t = await OpenConfirm(self, 200014, 200015, default,
                new ConfirmBtn(UIConfirmType.Ok, UIConfirmStyle.Red),
                new ConfirmBtn(UIConfirmType.Cancel));
            if (t == UIConfirmType.Ok)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        public static async ETTask<UIConfirmType> OpenConfirm(Entity entity, int title, int desc, UIConfirmExtra extra, params ConfirmBtn[] btns)
        {
            string t = LanguageCategory.Instance.Contain(title)? LanguageCategory.Instance.Get2(title).Msg : title.ToString();
            string d = LanguageCategory.Instance.Contain(desc)? LanguageCategory.Instance.Get2(desc).Msg : desc.ToString();
            return await OpenConfirm(entity, t, d, extra, btns);
        }

        public static async ETTask<UIConfirmType> OpenConfirm(Entity entity, string title, string desc, UIConfirmExtra extra,
        params ConfirmBtn[] btns)
        {
            UIConfirm win = entity.Root().GetComponent<UIComponent>().GetDlgLogic<UIConfirm>();
            if (win)
            {
                if (win.GetTipConfirmType(extra.TipType) != default)
                {
                    return win.GetTipConfirmType(extra.TipType);
                }
            }

            using UIConfirmData data = entity.Scene().AddComponent<UIConfirmData>();
            data.Title = title;
            data.Desc = desc;
            data.BtnList.Clear();
            data.BtnList.AddRange(btns);
            data.Extra = extra;
            await entity.Root().GetComponent<UIComponent>().ShowWindow<UIConfirm>(data);
            Wait_CloseConfirm w = await entity.Root().GetComponent<ObjectWait>().Wait<Wait_CloseConfirm>();

            return w.ConfirmType;
        }
    }
}