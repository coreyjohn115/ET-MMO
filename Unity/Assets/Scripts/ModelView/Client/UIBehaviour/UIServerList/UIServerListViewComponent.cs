using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class UIServerListViewComponent : Entity, IAwake, IDestroy 
	{
		public ExtendText E_TitleExtendText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_TitleExtendText == null )
     			{
		    		this.m_E_TitleExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"Panel/E_Title");
     			}
     			return this.m_E_TitleExtendText;
     		}
     	}

		public LoopVerticalScrollRect E_ServerListLoopVerticalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ServerListLoopVerticalScrollRect == null )
     			{
		    		this.m_E_ServerListLoopVerticalScrollRect = UIFindHelper.FindDeepChild<LoopVerticalScrollRect>(this.uiTransform.gameObject,"Panel/E_ServerList");
     			}
     			return this.m_E_ServerListLoopVerticalScrollRect;
     		}
     	}

		public Button E_CloseBtnButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CloseBtnButton == null )
     			{
		    		this.m_E_CloseBtnButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"Panel/E_CloseBtn");
     			}
     			return this.m_E_CloseBtnButton;
     		}
     	}

		public ExtendImage E_CloseBtnExtendImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CloseBtnExtendImage == null )
     			{
		    		this.m_E_CloseBtnExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"Panel/E_CloseBtn");
     			}
     			return this.m_E_CloseBtnExtendImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_TitleExtendText = null;
			this.m_E_ServerListLoopVerticalScrollRect = null;
			this.m_E_CloseBtnButton = null;
			this.m_E_CloseBtnExtendImage = null;
			uiTransform = null;
		}

		private ExtendText m_E_TitleExtendText = null;
		private LoopVerticalScrollRect m_E_ServerListLoopVerticalScrollRect = null;
		private Button m_E_CloseBtnButton = null;
		private ExtendImage m_E_CloseBtnExtendImage = null;
		public Transform uiTransform = null;
	}
}
