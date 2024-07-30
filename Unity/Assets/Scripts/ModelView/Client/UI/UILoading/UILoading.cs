namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	public  class UILoading : Entity, IAwake, IUILogic
	{
		public UILoadingViewComponent View { get => GetParent<UIBaseWindow>().GetComponent<UILoadingViewComponent>();} 
	}
}
