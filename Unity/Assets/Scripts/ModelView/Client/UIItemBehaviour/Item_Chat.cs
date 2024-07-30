using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EnableMethod]
	public partial class Scroll_Item_Chat : Entity, IAwake, IDestroy, IUIScrollItem 
	{
		public long DataId {get;set;}
		private bool isCacheNode = false;

		public void SetCacheMode(bool isCache)
		{
			this.isCacheNode = isCache;
		}

		public void DestroyWidget()
		{
			uiTransform = null;
			this.DataId = 0;
		}

		public Transform uiTransform = null;
	}
}
