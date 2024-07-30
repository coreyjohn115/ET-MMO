using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(ES_OtherPetHurt))]
	[FriendOf(typeof(ES_OtherPetHurt))]
	public static partial class ES_OtherPetHurtSystem 
	{
		[EntitySystem]
		private static void Awake(this ES_OtherPetHurt self, Transform transform)
		{
			self.uiTransform = transform;
		}

		[EntitySystem]
		private static void Destroy(this ES_OtherPetHurt self)
		{
			self.DestroyWidget();
		}
	}


}
