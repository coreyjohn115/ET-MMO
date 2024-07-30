using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(ES_ComAttr))]
	[FriendOf(typeof(ES_ComAttr))]
	public static partial class ES_ComAttrSystem 
	{
		[EntitySystem]
		private static void Awake(this ES_ComAttr self, Transform transform)
		{
			self.uiTransform = transform;
		}

		[EntitySystem]
		private static void Destroy(this ES_ComAttr self)
		{
			self.DestroyWidget();
		}
	}


}
