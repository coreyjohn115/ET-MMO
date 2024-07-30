namespace ET.Client
{
	[FriendOf(typeof(WindowCoreData))]
	[FriendOf(typeof(UIBaseWindow))]
	[UIEvent(WindowID.Win_UITools)]
	public  class UIToolsEventHandler : IUIEventHandler
	{
		public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.WindowData.WindowType = UIWindowType.Other; 
			uiBaseWindow.WindowData.IsStatic = true;
			uiBaseWindow.WindowData.NeedMask = false;
			uiBaseWindow.WindowData.TriggerFoucs = false;
		}

		public void OnInitComponent(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.AddComponent<UIToolsViewComponent>(); 
			uiBaseWindow.AddComponent<UITools>(); 
		}

		public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.GetComponent<UITools>().RegisterUIEvent(); 
		}

		public void OnReload(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.GetComponent<UITools>().ReloadUIEvent(); 
		}

		public void OnFocus(UIBaseWindow uiBaseWindow)
		{
		}

		public void OnUnFocus(UIBaseWindow uiBaseWindow)
		{
		}

		public void OnShowWindow(UIBaseWindow uiBaseWindow, Entity showData = null)
		{
			uiBaseWindow.GetComponent<UITools>().ShowWindow(showData); 
		}

		public void OnHideWindow(UIBaseWindow uiBaseWindow)
		{

		}

		public void BeforeUnload(UIBaseWindow uiBaseWindow)
		{

		}
	}
}
