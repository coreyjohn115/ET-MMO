namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	public  class UIHud : Entity, IAwake, IUILogic
	{
		public UIHudViewComponent View { get => GetParent<UIBaseWindow>().GetComponent<UIHudViewComponent>();} 
	}
}
