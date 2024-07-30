namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	public  class UIItemTip : Entity, IAwake, IUILogic
	{
		public UIItemTipViewComponent View { get => GetParent<UIBaseWindow>().GetComponent<UIItemTipViewComponent>();} 
	}
}
