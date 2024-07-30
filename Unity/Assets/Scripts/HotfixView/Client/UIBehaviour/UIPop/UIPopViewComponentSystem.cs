namespace ET.Client
{
	[FriendOf(typeof(UIPopViewComponent))]
	public class UIPopViewComponentAwakeSystem : AwakeSystem<UIPopViewComponent> 
	{
		protected override void Awake(UIPopViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().UITransform;
		}
	}


	[FriendOf(typeof(UIPopViewComponent))]
	public class UIPopViewComponentDestroySystem : DestroySystem<UIPopViewComponent> 
	{
		protected override void Destroy(UIPopViewComponent self)
		{
			self.DestroyWidget();

		}
	}
}
