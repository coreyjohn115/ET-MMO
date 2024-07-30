using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(Scroll_Item_Server))]
	[FriendOf(typeof(Scroll_Item_Server))]
	public static partial class Scroll_Item_ServerSystem 
	{
		[EntitySystem]
		private static void Awake(this Scroll_Item_Server self)
		{
		}

		[EntitySystem]
		private static void Destroy(this Scroll_Item_Server self)
		{
			self.DestroyWidget();
		}

		public static Scroll_Item_Server BindTrans(this Scroll_Item_Server self, Transform trans)
		{
			self.uiTransform = trans;
			return self;
		}
	}
}
