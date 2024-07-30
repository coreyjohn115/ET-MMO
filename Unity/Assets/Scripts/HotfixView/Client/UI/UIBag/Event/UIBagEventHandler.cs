namespace ET.Client
{
	[FriendOf(typeof(WindowCoreData))]
	[FriendOf(typeof(UIBaseWindow))]
	[UIEvent(WindowID.Win_UIBag)]
	public  class UIBagEventHandler : IUIEventHandler
	{
		public void OnInitWindowCoreData(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.WindowData.WindowType = UIWindowType.Normal;
		}

		public void OnInitComponent(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.AddComponent<UIBagViewComponent>(); 
			uiBaseWindow.AddComponent<UIBag>(); 
		}

		public void OnRegisterUIEvent(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.GetComponent<UIBag>().RegisterUIEvent(); 
		}

		public void OnReload(UIBaseWindow uiBaseWindow)
		{
			uiBaseWindow.GetComponent<UIBag>().ReloadUIEvent(); 
		}

		public void OnFocus(UIBaseWindow uiBaseWindow)
		{
		}

		public void OnUnFocus(UIBaseWindow uiBaseWindow)
		{
		}

		public void OnShowWindow(UIBaseWindow uiBaseWindow, Entity showData = null)
		{
			uiBaseWindow.GetComponent<UIBag>().ShowWindow(showData); 
		}

		public void OnHideWindow(UIBaseWindow uiBaseWindow)
		{

		}

		public void BeforeUnload(UIBaseWindow uiBaseWindow)
		{

		}
	}
}
