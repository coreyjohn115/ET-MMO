namespace ET.Client
{
	[FriendOf(typeof(UIGmViewComponent))]
	public class UIGmViewComponentAwakeSystem : AwakeSystem<UIGmViewComponent> 
	{
		protected override void Awake(UIGmViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().UITransform;
		}
	}


	[FriendOf(typeof(UIGmViewComponent))]
	public class UIGmViewComponentDestroySystem : DestroySystem<UIGmViewComponent> 
	{
		protected override void Destroy(UIGmViewComponent self)
		{
			self.DestroyWidget();

		}
	}
}
