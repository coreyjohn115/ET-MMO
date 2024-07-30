using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(ES_SelfPetHurtCrit))]
	[FriendOf(typeof(ES_SelfPetHurtCrit))]
	public static partial class ES_SelfPetHurtCritSystem 
	{
		[EntitySystem]
		private static void Awake(this ES_SelfPetHurtCrit self, Transform transform)
		{
			self.uiTransform = transform;
		}

		[EntitySystem]
		private static void Destroy(this ES_SelfPetHurtCrit self)
		{
			self.DestroyWidget();
		}
	}


}
