namespace ET.Client
{
    [FriendOf(typeof (WindowCoreData))]
    [FriendOf(typeof (UIBaseWindow))]
    [UIEvent(WindowID.Win_UIHud)]
    public class UIHudEventHandler: IUIEventHandler
    {
        public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.WindowData.WindowType = UIWindowType.Normal;
            uiBaseWindow.WindowData.NeedMask = false;
            uiBaseWindow.WindowData.TriggerFoucs = false;
            uiBaseWindow.WindowData.IsStatic = true;
        }

        public void OnInitComponent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.AddComponent<UIHudViewComponent>();
            uiBaseWindow.AddComponent<UIHud>();
        }

        public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<UIHud>().RegisterUIEvent();
        }

        public void OnReload(UIBaseWindow uiBaseWindow)
        {
        }

        public void OnFocus(UIBaseWindow uiBaseWindow)
        {
        }

        public void OnUnFocus(UIBaseWindow uiBaseWindow)
        {
        }

        public void OnShowWindow(UIBaseWindow uiBaseWindow, Entity showData = null)
        {
            uiBaseWindow.GetComponent<UIHud>().ShowWindow(showData);
        }

        public void OnHideWindow(UIBaseWindow uiBaseWindow)
        {
        }

        public void BeforeUnload(UIBaseWindow uiBaseWindow)
        {
        }
    }
}