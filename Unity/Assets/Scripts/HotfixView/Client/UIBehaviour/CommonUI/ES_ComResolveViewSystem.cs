using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(ES_ComResolve))]
	[FriendOf(typeof(ES_ComResolve))]
	public static partial class ES_ComResolveSystem 
	{
		[EntitySystem]
		private static void Awake(this ES_ComResolve self, Transform transform)
		{
			self.uiTransform = transform;
		}

		[EntitySystem]
		private static void Destroy(this ES_ComResolve self)
		{
			self.DestroyWidget();
		}
	}


}
