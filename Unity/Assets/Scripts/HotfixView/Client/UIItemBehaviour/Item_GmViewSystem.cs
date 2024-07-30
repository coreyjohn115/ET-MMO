using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(Scroll_Item_Gm))]
	[FriendOf(typeof(Scroll_Item_Gm))]
	public static partial class Scroll_Item_GmSystem 
	{
		[EntitySystem]
		private static void Awake(this Scroll_Item_Gm self)
		{
		}

		[EntitySystem]
		private static void Destroy(this Scroll_Item_Gm self)
		{
			self.DestroyWidget();
		}

		public static Scroll_Item_Gm BindTrans(this Scroll_Item_Gm self, Transform trans)
		{
			self.uiTransform = trans;
			return self;
		}
	}
}
