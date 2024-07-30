namespace ET.Client
{
    [FriendOf(typeof (WindowCoreData))]
    [FriendOf(typeof (UIBaseWindow))]
    [UIEvent(WindowID.Win_UIChat)]
    public class UIChatEventHandler: IUIEventHandler
    {
        public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.WindowData.WindowType = UIWindowType.Normal;
        }

        public void OnInitComponent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.AddComponent<UIChatViewComponent>();
            uiBaseWindow.AddComponent<UIChat>();
        }

        public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<UIChat>().RegisterUIEvent();
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
            uiBaseWindow.GetComponent<UIChat>().ShowWindow(showData);
        }

        public void OnHideWindow(UIBaseWindow uiBaseWindow)
        {
            
        }

        public void BeforeUnload(UIBaseWindow uiBaseWindow)
        {
            uiBaseWindow.GetComponent<UIChat>().BeforeUnload();
        }
    }
}