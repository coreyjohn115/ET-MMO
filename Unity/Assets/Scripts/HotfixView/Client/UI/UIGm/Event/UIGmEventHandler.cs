namespace ET.Client
{
    [FriendOf(typeof (WindowCoreData))]
    [FriendOf(typeof (UIBaseWindow))]
    [UIEvent(WindowID.Win_UIGm)]
    public class UIGmEventHandler: IUIEventHandler
    {
        public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.WindowData.WindowType = UIWindowType.Other;
            uiBaseWindow.WindowData.TriggerFocus = false;
            uiBaseWindow.WindowData.CheckDispose = false;
        }

        public void OnInitComponent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.AddComponent<UIGmViewComponent>();
            uiBaseWindow.AddComponent<UIGm>();
        }

        public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<UIGm>().RegisterUIEvent();
        }

        public void OnReload(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<UIGm>().ReloadUIEvent();
        }

        public void OnFocus(UIBaseWindow uiBaseWindow)
        {
        }

        public void OnUnFocus(UIBaseWindow uiBaseWindow)
        {
        }

        public void OnShowWindow(UIBaseWindow uiBaseWindow, Entity showData = null)
        {
            uiBaseWindow.GetComponent<UIGm>().ShowWindow(showData);
        }

        public void OnHideWindow(UIBaseWindow uiBaseWindow)
        {
        }

        public void BeforeUnload(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<UIGm>().BeforeUnload();
        }
    }
}