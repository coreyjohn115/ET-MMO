namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	public  class UIMain : Entity, IAwake, IUILogic
	{
		public UIMainViewComponent View { get => GetParent<UIBaseWindow>().GetComponent<UIMainViewComponent>();} 
	}
}
