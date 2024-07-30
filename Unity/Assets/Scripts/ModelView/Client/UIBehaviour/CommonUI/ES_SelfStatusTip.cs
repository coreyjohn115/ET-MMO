using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ChildOf]
	[EnableMethod]
	public partial class ES_SelfStatusTip : Entity, ET.IAwake<Transform>, IDestroy 
	{
		public void DestroyWidget()
		{
			uiTransform = null;
		}

		public Transform uiTransform = null;
	}
}
