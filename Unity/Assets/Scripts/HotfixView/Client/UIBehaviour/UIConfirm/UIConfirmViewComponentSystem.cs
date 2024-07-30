namespace ET.Client
{
	[FriendOf(typeof(UIConfirmViewComponent))]
	public class UIConfirmViewComponentAwakeSystem : AwakeSystem<UIConfirmViewComponent> 
	{
		protected override void Awake(UIConfirmViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().UITransform;
		}
	}


	[FriendOf(typeof(UIConfirmViewComponent))]
	public class UIConfirmViewComponentDestroySystem : DestroySystem<UIConfirmViewComponent> 
	{
		protected override void Destroy(UIConfirmViewComponent self)
		{
			self.DestroyWidget();

		}
	}
}
