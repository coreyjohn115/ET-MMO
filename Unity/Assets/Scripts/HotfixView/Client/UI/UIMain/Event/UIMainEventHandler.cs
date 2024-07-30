namespace ET.Client
{
	[FriendOf(typeof(WindowCoreData))]
	[FriendOf(typeof(UIBaseWindow))]
	[UIEvent(WindowID.Win_UIMain)]
	public  class UIMainEventHandler : IUIEventHandler
	{
		public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.WindowData.WindowType = UIWindowType.Normal;
			uiBaseWindow.WindowData.NeedMask = false;
			uiBaseWindow.WindowData.IsStatic = true;
		}

		public void OnInitComponent(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.AddComponent<UIMainViewComponent>(); 
			uiBaseWindow.AddComponent<UIMain>(); 
		}

		public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.GetComponent<UIMain>().RegisterUIEvent(); 
		}

		public void OnReload(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.GetComponent<UIMain>().ReloadUIEvent();
		}

		public void OnFocus(UIBaseWindow uiBaseWindow)
		{
			
		}

		public void OnUnFocus(UIBaseWindow uiBaseWindow)
		{
			
		}

		public void OnShowWindow(UIBaseWindow uiBaseWindow, Entity showData = null)
		{
			uiBaseWindow.GetComponent<UIMain>().ShowWindow(showData); 
		}

		public void OnHideWindow(UIBaseWindow uiBaseWindow)
		{

		}

		public void BeforeUnload(UIBaseWindow uiBaseWindow)
		{

		}
	}
}
