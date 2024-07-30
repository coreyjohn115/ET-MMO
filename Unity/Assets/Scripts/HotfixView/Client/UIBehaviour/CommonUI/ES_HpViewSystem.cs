using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(ES_Hp))]
	[FriendOf(typeof(ES_Hp))]
	public static partial class ES_HpSystem 
	{
		[EntitySystem]
		private static void Awake(this ES_Hp self, Transform transform)
		{
			self.uiTransform = transform;
		}

		[EntitySystem]
		private static void Destroy(this ES_Hp self)
		{
			self.DestroyWidget();
		}
	}


}
