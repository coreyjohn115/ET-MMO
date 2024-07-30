namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	public  class UITools : Entity, IAwake, IUILogic
	{
		public UIToolsViewComponent View { get => GetParent<UIBaseWindow>().GetComponent<UIToolsViewComponent>();} 
	}
}
