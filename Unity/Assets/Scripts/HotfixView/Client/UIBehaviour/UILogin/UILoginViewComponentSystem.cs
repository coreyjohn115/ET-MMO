namespace ET.Client
{
	[FriendOf(typeof(UILoginViewComponent))]
	public class UILoginViewComponentAwakeSystem : AwakeSystem<UILoginViewComponent> 
	{
		protected override void Awake(UILoginViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().UITransform;
		}
	}


	[FriendOf(typeof(UILoginViewComponent))]
	public class UILoginViewComponentDestroySystem : DestroySystem<UILoginViewComponent> 
	{
		protected override void Destroy(UILoginViewComponent self)
		{
			self.DestroyWidget();

		}
	}
}
