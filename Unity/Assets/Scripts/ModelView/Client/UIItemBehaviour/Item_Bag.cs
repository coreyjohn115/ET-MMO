using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EnableMethod]
	public partial class Scroll_Item_Bag : Entity, IAwake, IDestroy, IUIScrollItem 
	{
		public long DataId {get;set;}
		private bool isCacheNode = false;

		public void SetCacheMode(bool isCache)
		{
			this.isCacheNode = isCache;
		}

		public ESItem ESItem
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_esitem == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"ESItem");
		    	   this.m_esitem = this.AddChild<ESItem,Transform>(subTrans);
     			}
     			return this.m_esitem;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_esitem = null;
			uiTransform = null;
			this.DataId = 0;
		}

		private EntityRef<ESItem> m_esitem = null;
		public Transform uiTransform = null;
	}
}
