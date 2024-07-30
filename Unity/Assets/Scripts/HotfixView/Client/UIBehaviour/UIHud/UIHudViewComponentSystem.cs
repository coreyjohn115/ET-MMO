namespace ET.Client
{
	[FriendOf(typeof(UIHudViewComponent))]
	public class UIHudViewComponentAwakeSystem : AwakeSystem<UIHudViewComponent> 
	{
		protected override void Awake(UIHudViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().UITransform;
		}
	}


	[FriendOf(typeof(UIHudViewComponent))]
	public class UIHudViewComponentDestroySystem : DestroySystem<UIHudViewComponent> 
	{
		protected override void Destroy(UIHudViewComponent self)
		{
			self.DestroyWidget();

		}
	}
}
