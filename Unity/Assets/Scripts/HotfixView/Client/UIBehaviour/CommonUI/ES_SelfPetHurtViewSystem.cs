using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(ES_SelfPetHurt))]
	[FriendOf(typeof(ES_SelfPetHurt))]
	public static partial class ES_SelfPetHurtSystem 
	{
		[EntitySystem]
		private static void Awake(this ES_SelfPetHurt self, Transform transform)
		{
			self.uiTransform = transform;
		}

		[EntitySystem]
		private static void Destroy(this ES_SelfPetHurt self)
		{
			self.DestroyWidget();
		}
	}


}
