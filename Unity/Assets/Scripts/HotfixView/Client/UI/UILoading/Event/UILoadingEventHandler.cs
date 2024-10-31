namespace ET.Client
{
    [FriendOf(typeof (WindowCoreData))]
    [FriendOf(typeof (UIBaseWindow))]
    [UIEvent(WindowID.Win_UILoading)]
    public class UILoadingEventHandler: IUIEventHandler
    {
        public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.WindowData.WindowType = UIWindowType.Fixed;
            uiBaseWindow.WindowData.NeedMask = false;
            uiBaseWindow.WindowData.TriggerFocus = false;
        }

        public void OnInitComponent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.AddComponent<UILoadingViewComponent>();
            uiBaseWindow.AddComponent<UILoading>();
        }

        public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<UILoading>().RegisterUIEvent();
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
            uiBaseWindow.GetComponent<UILoading>().ShowWindow(showData);
        }

        public void OnHideWindow(UIBaseWindow uiBaseWindow)
        {
        }

        public void BeforeUnload(UIBaseWindow uiBaseWindow)
        {
        }
    }
}