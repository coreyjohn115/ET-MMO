namespace ET.Client
{
	[FriendOf(typeof(UIChatViewComponent))]
	public class UIChatViewComponentAwakeSystem : AwakeSystem<UIChatViewComponent> 
	{
		protected override void Awake(UIChatViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().UITransform;
		}
	}


	[FriendOf(typeof(UIChatViewComponent))]
	public class UIChatViewComponentDestroySystem : DestroySystem<UIChatViewComponent> 
	{
		protected override void Destroy(UIChatViewComponent self)
		{
			self.DestroyWidget();

		}
	}
}
