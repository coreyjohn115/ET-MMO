using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class UIMainViewComponent : Entity, IAwake, IDestroy 
	{
		public Button E_ChatButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ChatButton == null )
     			{
		    		this.m_E_ChatButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"E_Chat");
     			}
     			return this.m_E_ChatButton;
     		}
     	}

		public ExtendImage E_ChatExtendImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ChatExtendImage == null )
     			{
		    		this.m_E_ChatExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"E_Chat");
     			}
     			return this.m_E_ChatExtendImage;
     		}
     	}

		public LoopVerticalScrollRect E_BottomMenuListLoopVerticalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_BottomMenuListLoopVerticalScrollRect == null )
     			{
		    		this.m_E_BottomMenuListLoopVerticalScrollRect = UIFindHelper.FindDeepChild<LoopVerticalScrollRect>(this.uiTransform.gameObject,"E_BottomMenuList");
     			}
     			return this.m_E_BottomMenuListLoopVerticalScrollRect;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_ChatButton = null;
			this.m_E_ChatExtendImage = null;
			this.m_E_BottomMenuListLoopVerticalScrollRect = null;
			uiTransform = null;
		}

		private Button m_E_ChatButton = null;
		private ExtendImage m_E_ChatExtendImage = null;
		private LoopVerticalScrollRect m_E_BottomMenuListLoopVerticalScrollRect = null;
		public Transform uiTransform = null;
	}
}
