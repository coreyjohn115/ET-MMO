namespace ET.Client
{
	[FriendOf(typeof(UIToolsViewComponent))]
	public class UIToolsViewComponentAwakeSystem : AwakeSystem<UIToolsViewComponent> 
	{
		protected override void Awake(UIToolsViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().UITransform;
		}
	}


	[FriendOf(typeof(UIToolsViewComponent))]
	public class UIToolsViewComponentDestroySystem : DestroySystem<UIToolsViewComponent> 
	{
		protected override void Destroy(UIToolsViewComponent self)
		{
			self.DestroyWidget();

		}
	}
}
