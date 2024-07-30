using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class UIGmViewComponent : Entity, IAwake, IDestroy 
	{
		public Transform UiTransform => this.uiTransform;

		public LoopVerticalScrollRect E_MenuLoopVerticalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_MenuLoopVerticalScrollRect == null )
     			{
		    		this.m_E_MenuLoopVerticalScrollRect = UIFindHelper.FindDeepChild<LoopVerticalScrollRect>(this.uiTransform.gameObject,"E_Menu");
     			}
     			return this.m_E_MenuLoopVerticalScrollRect;
     		}
     	}

		public LoopVerticalScrollRect E_SubMenuLoopVerticalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_SubMenuLoopVerticalScrollRect == null )
     			{
		    		this.m_E_SubMenuLoopVerticalScrollRect = UIFindHelper.FindDeepChild<LoopVerticalScrollRect>(this.uiTransform.gameObject,"E_SubMenu");
     			}
     			return this.m_E_SubMenuLoopVerticalScrollRect;
     		}
     	}

		public ExtendText E_DescExtendText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_DescExtendText == null )
     			{
		    		this.m_E_DescExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"Right/E_Desc");
     			}
     			return this.m_E_DescExtendText;
     		}
     	}

		public InputField E_InputInputField
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_InputInputField == null )
     			{
		    		this.m_E_InputInputField = UIFindHelper.FindDeepChild<InputField>(this.uiTransform.gameObject,"Right/E_Input");
     			}
     			return this.m_E_InputInputField;
     		}
     	}

		public Button E_ClickButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ClickButton == null )
     			{
		    		this.m_E_ClickButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"Right/E_Click");
     			}
     			return this.m_E_ClickButton;
     		}
     	}

		public ExtendImage E_ClickExtendImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ClickExtendImage == null )
     			{
		    		this.m_E_ClickExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"Right/E_Click");
     			}
     			return this.m_E_ClickExtendImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_MenuLoopVerticalScrollRect = null;
			this.m_E_SubMenuLoopVerticalScrollRect = null;
			this.m_E_DescExtendText = null;
			this.m_E_InputInputField = null;
			this.m_E_ClickButton = null;
			this.m_E_ClickExtendImage = null;
			uiTransform = null;
		}

		private LoopVerticalScrollRect m_E_MenuLoopVerticalScrollRect = null;
		private LoopVerticalScrollRect m_E_SubMenuLoopVerticalScrollRect = null;
		private ExtendText m_E_DescExtendText = null;
		private InputField m_E_InputInputField = null;
		private Button m_E_ClickButton = null;
		private ExtendImage m_E_ClickExtendImage = null;
		public Transform uiTransform = null;
	}
}
