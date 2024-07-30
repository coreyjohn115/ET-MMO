namespace ET.Client
{
	[FriendOf(typeof(UILoadingViewComponent))]
	public class UILoadingViewComponentAwakeSystem : AwakeSystem<UILoadingViewComponent> 
	{
		protected override void Awake(UILoadingViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().UITransform;
		}
	}


	[FriendOf(typeof(UILoadingViewComponent))]
	public class UILoadingViewComponentDestroySystem : DestroySystem<UILoadingViewComponent> 
	{
		protected override void Destroy(UILoadingViewComponent self)
		{
			self.DestroyWidget();

		}
	}
}
