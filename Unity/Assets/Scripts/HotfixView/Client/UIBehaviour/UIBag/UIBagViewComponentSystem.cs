namespace ET.Client
{
	[FriendOf(typeof(UIBagViewComponent))]
	public class UIBagViewComponentAwakeSystem : AwakeSystem<UIBagViewComponent> 
	{
		protected override void Awake(UIBagViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().UITransform;
			self.RegisterCloseEvent(self.E_CloseBtnButton);
		}
	}


	[FriendOf(typeof(UIBagViewComponent))]
	public class UIBagViewComponentDestroySystem : DestroySystem<UIBagViewComponent> 
	{
		protected override void Destroy(UIBagViewComponent self)
		{
			self.DestroyWidget();

		}
	}
}
