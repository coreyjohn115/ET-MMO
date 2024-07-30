namespace ET.Client
{
	[FriendOf(typeof(UIServerListViewComponent))]
	public class UIServerListViewComponentAwakeSystem : AwakeSystem<UIServerListViewComponent> 
	{
		protected override void Awake(UIServerListViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().UITransform;
			self.RegisterCloseEvent(self.E_CloseBtnButton);
		}
	}


	[FriendOf(typeof(UIServerListViewComponent))]
	public class UIServerListViewComponentDestroySystem : DestroySystem<UIServerListViewComponent> 
	{
		protected override void Destroy(UIServerListViewComponent self)
		{
			self.DestroyWidget();

		}
	}
}
