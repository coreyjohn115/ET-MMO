using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EnableMethod]
	public partial class Scroll_Item_BagMenu : Entity, IAwake, IDestroy, IUIScrollItem 
	{
		public long DataId {get;set;}
		private bool isCacheNode = false;

		public void SetCacheMode(bool isCache)
		{
			this.isCacheNode = isCache;
		}

		public ExtendImage E_SelectExtendImage
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
     				if( this.m_E_SelectExtendImage == null )
     				{
		    			this.m_E_SelectExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"E_Select");
     				}
     				return this.m_E_SelectExtendImage;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"E_Select");
     			}
     		}
     	}

		public ExtendText E_NameExtendText
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
     				if( this.m_E_NameExtendText == null )
     				{
		    			this.m_E_NameExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Name");
     				}
     				return this.m_E_NameExtendText;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Name");
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
			this.m_E_SelectExtendImage = null;
			this.m_E_NameExtendText = null;
			this.m_E_BtnButton = null;
			uiTransform = null;
			this.DataId = 0;
		}

		private ExtendImage m_E_SelectExtendImage = null;
		private ExtendText m_E_NameExtendText = null;
		private Button m_E_BtnButton = null;
		public Transform uiTransform = null;
	}
}
