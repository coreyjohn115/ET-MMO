namespace ET.Client
{
	[FriendOf(typeof(WindowCoreData))]
	[FriendOf(typeof(UIBaseWindow))]
	[UIEvent(WindowID.Win_UIConfirm)]
	public  class UIConfirmEventHandler : IUIEventHandler
	{
		public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.WindowData.WindowType = UIWindowType.Normal;
		}

		public void OnInitComponent(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.AddComponent<UIConfirmViewComponent>(); 
			uiBaseWindow.AddComponent<UIConfirm>(); 
		}

		public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.GetComponent<UIConfirm>().RegisterUIEvent();
		}

		public void OnReload(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.GetComponent<UIConfirm>().ReloadUIEvent();
		}

		public void OnFocus(UIBaseWindow uiBaseWindow)
		{
		}

		public void OnUnFocus(UIBaseWindow uiBaseWindow)
		{
		}

		public void OnShowWindow(UIBaseWindow uiBaseWindow, Entity showData = null)
		{
			uiBaseWindow.GetComponent<UIConfirm>().ShowWindow(showData);
		}

		public void OnHideWindow(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.GetComponent<UIConfirm>().HideWindow();
		}

		public void BeforeUnload(UIBaseWindow uiBaseWindow)
		{

		}
	}
}
