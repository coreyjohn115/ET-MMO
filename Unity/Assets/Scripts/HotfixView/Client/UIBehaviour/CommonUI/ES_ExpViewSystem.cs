using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(ES_Exp))]
	[FriendOf(typeof(ES_Exp))]
	public static partial class ES_ExpSystem 
	{
		[EntitySystem]
		private static void Awake(this ES_Exp self, Transform transform)
		{
			self.uiTransform = transform;
		}

		[EntitySystem]
		private static void Destroy(this ES_Exp self)
		{
			self.DestroyWidget();
		}
	}


}
