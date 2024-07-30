using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(ES_OtherPetHurtCrit))]
	[FriendOf(typeof(ES_OtherPetHurtCrit))]
	public static partial class ES_OtherPetHurtCritSystem 
	{
		[EntitySystem]
		private static void Awake(this ES_OtherPetHurtCrit self, Transform transform)
		{
			self.uiTransform = transform;
		}

		[EntitySystem]
		private static void Destroy(this ES_OtherPetHurtCrit self)
		{
			self.DestroyWidget();
		}
	}


}
