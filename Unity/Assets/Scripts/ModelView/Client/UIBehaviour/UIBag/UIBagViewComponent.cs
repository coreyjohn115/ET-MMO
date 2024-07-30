using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class UIBagViewComponent : Entity, IAwake, IDestroy 
	{
		public Transform UiTransform => this.uiTransform;

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
		    		this.m_E_TitleExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Title");
     			}
     			return this.m_E_TitleExtendText;
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
		    		this.m_E_CloseBtnButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"E_CloseBtn");
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
		    		this.m_E_CloseBtnExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"E_CloseBtn");
     			}
     			return this.m_E_CloseBtnExtendImage;
     		}
     	}

		public LoopVerticalScrollRect E_BagMenuListLoopVerticalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_BagMenuListLoopVerticalScrollRect == null )
     			{
		    		this.m_E_BagMenuListLoopVerticalScrollRect = UIFindHelper.FindDeepChild<LoopVerticalScrollRect>(this.uiTransform.gameObject,"E_BagMenuList");
     			}
     			return this.m_E_BagMenuListLoopVerticalScrollRect;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_TitleExtendText = null;
			this.m_E_CloseBtnButton = null;
			this.m_E_CloseBtnExtendImage = null;
			this.m_E_BagMenuListLoopVerticalScrollRect = null;
			uiTransform = null;
		}

		private ExtendText m_E_TitleExtendText = null;
		private Button m_E_CloseBtnButton = null;
		private ExtendImage m_E_CloseBtnExtendImage = null;
		private LoopVerticalScrollRect m_E_BagMenuListLoopVerticalScrollRect = null;
		public Transform uiTransform = null;
	}
}
