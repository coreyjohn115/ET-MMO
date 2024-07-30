using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(Scroll_Item_EmotionMenu))]
	[FriendOf(typeof(Scroll_Item_EmotionMenu))]
	public static partial class Scroll_Item_EmotionMenuSystem 
	{
		[EntitySystem]
		private static void Awake(this Scroll_Item_EmotionMenu self)
		{
		}

		[EntitySystem]
		private static void Destroy(this Scroll_Item_EmotionMenu self)
		{
			self.DestroyWidget();
		}

		public static Scroll_Item_EmotionMenu BindTrans(this Scroll_Item_EmotionMenu self, Transform trans)
		{
			self.uiTransform = trans;
			return self;
		}
	}
}
