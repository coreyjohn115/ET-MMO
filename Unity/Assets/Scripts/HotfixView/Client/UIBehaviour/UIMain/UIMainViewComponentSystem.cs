namespace ET.Client
{
	[FriendOf(typeof(UIMainViewComponent))]
	public class UIMainViewComponentAwakeSystem : AwakeSystem<UIMainViewComponent> 
	{
		protected override void Awake(UIMainViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().UITransform;
		}
	}


	[FriendOf(typeof(UIMainViewComponent))]
	public class UIMainViewComponentDestroySystem : DestroySystem<UIMainViewComponent> 
	{
		protected override void Destroy(UIMainViewComponent self)
		{
			self.DestroyWidget();

		}
	}
}
