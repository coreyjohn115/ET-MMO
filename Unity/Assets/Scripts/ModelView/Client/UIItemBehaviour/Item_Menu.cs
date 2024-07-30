using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EnableMethod]
	public partial class Scroll_Item_Menu : Entity, IAwake, IDestroy, IUIScrollItem 
	{
		public long DataId {get;set;}
		private bool isCacheNode = false;

		public void SetCacheMode(bool isCache)
		{
			this.isCacheNode = isCache;
		}

		public RectTransform EG_SelectRectTransform
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
     				if( this.m_EG_SelectRectTransform == null )
     				{
		    			this.m_EG_SelectRectTransform = UIFindHelper.FindDeepChild<RectTransform>(this.uiTransform.gameObject,"EG_Select");
     				}
     				return this.m_EG_SelectRectTransform;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<RectTransform>(this.uiTransform.gameObject,"EG_Select");
     			}
     		}
     	}

		public ExtendText E_TextExtendText
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
     				if( this.m_E_TextExtendText == null )
     				{
		    			this.m_E_TextExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Text");
     				}
     				return this.m_E_TextExtendText;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Text");
     			}
     		}
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

		public RectTransform EG_LockRectTransform
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
     				if( this.m_EG_LockRectTransform == null )
     				{
		    			this.m_EG_LockRectTransform = UIFindHelper.FindDeepChild<RectTransform>(this.uiTransform.gameObject,"EG_Lock");
     				}
     				return this.m_EG_LockRectTransform;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<RectTransform>(this.uiTransform.gameObject,"EG_Lock");
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
			this.m_EG_SelectRectTransform = null;
			this.m_E_TextExtendText = null;
			this.m_E_IconExtendImage = null;
			this.m_EG_LockRectTransform = null;
			this.m_E_BtnButton = null;
			uiTransform = null;
			this.DataId = 0;
		}

		private RectTransform m_EG_SelectRectTransform = null;
		private ExtendText m_E_TextExtendText = null;
		private ExtendImage m_E_IconExtendImage = null;
		private RectTransform m_EG_LockRectTransform = null;
		private Button m_E_BtnButton = null;
		public Transform uiTransform = null;
	}
}
