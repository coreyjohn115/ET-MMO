using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EnableMethod]
	public partial class Scroll_Item_Emoj : Entity, IAwake, IDestroy, IUIScrollItem 
	{
		public long DataId {get;set;}
		private bool isCacheNode = false;

		public void SetCacheMode(bool isCache)
		{
			this.isCacheNode = isCache;
		}

		public ExtendImage E_IconExtendImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_E_IconExtendImage == null )
     				{
		    			this.m_E_IconExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"E_Icon");
     				}
     				return this.m_E_IconExtendImage;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"E_Icon");
     			}
     		}
     	}

		public Button E_BtnButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_E_BtnButton == null )
     				{
		    			this.m_E_BtnButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"E_Btn");
     				}
     				return this.m_E_BtnButton;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"E_Btn");
     			}
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_IconExtendImage = null;
			this.m_E_BtnButton = null;
			uiTransform = null;
			this.DataId = 0;
		}

		private ExtendImage m_E_IconExtendImage = null;
		private Button m_E_BtnButton = null;
		public Transform uiTransform = null;
	}
}
