using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(ES_SelfStatusTip))]
	[FriendOf(typeof(ES_SelfStatusTip))]
	public static partial class ES_SelfStatusTipSystem 
	{
		[EntitySystem]
		private static void Awake(this ES_SelfStatusTip self, Transform transform)
		{
			self.uiTransform = transform;
		}

		[EntitySystem]
		private static void Destroy(this ES_SelfStatusTip self)
		{
			self.DestroyWidget();
		}
	}


}
