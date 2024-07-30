namespace ET.Client
{
	[FriendOf(typeof(UIItemTipViewComponent))]
	public class UIItemTipViewComponentAwakeSystem : AwakeSystem<UIItemTipViewComponent> 
	{
		protected override void Awake(UIItemTipViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().UITransform;
		}
	}


	[FriendOf(typeof(UIItemTipViewComponent))]
	public class UIItemTipViewComponentDestroySystem : DestroySystem<UIItemTipViewComponent> 
	{
		protected override void Destroy(UIItemTipViewComponent self)
		{
			self.DestroyWidget();

		}
	}
}
