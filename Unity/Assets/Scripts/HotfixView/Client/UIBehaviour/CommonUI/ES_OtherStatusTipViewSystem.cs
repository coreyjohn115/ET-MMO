using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(ES_OtherStatusTip))]
	[FriendOf(typeof(ES_OtherStatusTip))]
	public static partial class ES_OtherStatusTipSystem 
	{
		[EntitySystem]
		private static void Awake(this ES_OtherStatusTip self, Transform transform)
		{
			self.uiTransform = transform;
		}

		[EntitySystem]
		private static void Destroy(this ES_OtherStatusTip self)
		{
			self.DestroyWidget();
		}
	}


}
