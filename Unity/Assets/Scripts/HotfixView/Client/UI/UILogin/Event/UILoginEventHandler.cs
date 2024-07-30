namespace ET.Client
{
	[FriendOf(typeof(WindowCoreData))]
	[FriendOf(typeof(UIBaseWindow))]
	[UIEvent(WindowID.Win_UILogin)]
	public  class UILoginEventHandler : IUIEventHandler
	{
		public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.WindowData.WindowType = UIWindowType.Normal; 
			uiBaseWindow.WindowData.IsStatic = true;
		}

		public void OnInitComponent(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.AddComponent<UILoginViewComponent>(); 
			uiBaseWindow.AddComponent<UILogin>(); 
		}

		public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.GetComponent<UILogin>().RegisterUIEvent(); 
		}

		public void OnReload(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.GetComponent<UILogin>().ReloadUIEvent(); 
		}

		public void OnFocus(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.GetComponent<UILogin>().OnFocus(); 
		}

		public void OnUnFocus(UIBaseWindow uiBaseWindow)
		{
			
		}

		public void OnShowWindow(UIBaseWindow uiBaseWindow, Entity showData = null)
		{
			uiBaseWindow.GetComponent<UILogin>().ShowWindow(showData); 
		}

		public void OnHideWindow(UIBaseWindow uiBaseWindow)
		{
			 
		}

		public void BeforeUnload(UIBaseWindow uiBaseWindow)
		{

		}
	}
}
