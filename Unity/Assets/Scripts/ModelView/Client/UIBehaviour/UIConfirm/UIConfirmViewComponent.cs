using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class UIConfirmViewComponent : Entity, IAwake, IDestroy 
	{
		public Button E_EmptyCloseButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_EmptyCloseButton == null )
     			{
		    		this.m_E_EmptyCloseButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"E_EmptyClose");
     			}
     			return this.m_E_EmptyCloseButton;
     		}
     	}

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
		    		this.m_E_TitleExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"Content/E_Title");
     			}
     			return this.m_E_TitleExtendText;
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
		    		this.m_E_DescExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"Content/Msg/E_Desc");
     			}
     			return this.m_E_DescExtendText;
     		}
     	}

		public Toggle E_ToggleToggle
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ToggleToggle == null )
     			{
		    		this.m_E_ToggleToggle = UIFindHelper.FindDeepChild<Toggle>(this.uiTransform.gameObject,"Content/Msg/E_Toggle");
     			}
     			return this.m_E_ToggleToggle;
     		}
     	}

		public Button E_CancelButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CancelButton == null )
     			{
		    		this.m_E_CancelButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"Content/Btns/E_Cancel");
     			}
     			return this.m_E_CancelButton;
     		}
     	}

		public ExtendText E_CancelTextExtendText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CancelTextExtendText == null )
     			{
		    		this.m_E_CancelTextExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"Content/Btns/E_Cancel/E_CancelText");
     			}
     			return this.m_E_CancelTextExtendText;
     		}
     	}

		public Button E_OkButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_OkButton == null )
     			{
		    		this.m_E_OkButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"Content/Btns/E_Ok");
     			}
     			return this.m_E_OkButton;
     		}
     	}

		public ExtendText E_OkTextExtendText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_OkTextExtendText == null )
     			{
		    		this.m_E_OkTextExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"Content/Btns/E_Ok/E_OkText");
     			}
     			return this.m_E_OkTextExtendText;
     		}
     	}

		public Button E_CloseButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CloseButton == null )
     			{
		    		this.m_E_CloseButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"Content/E_Close");
     			}
     			return this.m_E_CloseButton;
     		}
     	}

		public ExtendImage E_CloseExtendImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CloseExtendImage == null )
     			{
		    		this.m_E_CloseExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"Content/E_Close");
     			}
     			return this.m_E_CloseExtendImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_EmptyCloseButton = null;
			this.m_E_TitleExtendText = null;
			this.m_E_DescExtendText = null;
			this.m_E_ToggleToggle = null;
			this.m_E_CancelButton = null;
			this.m_E_CancelTextExtendText = null;
			this.m_E_OkButton = null;
			this.m_E_OkTextExtendText = null;
			this.m_E_CloseButton = null;
			this.m_E_CloseExtendImage = null;
			uiTransform = null;
		}

		private Button m_E_EmptyCloseButton = null;
		private ExtendText m_E_TitleExtendText = null;
		private ExtendText m_E_DescExtendText = null;
		private Toggle m_E_ToggleToggle = null;
		private Button m_E_CancelButton = null;
		private ExtendText m_E_CancelTextExtendText = null;
		private Button m_E_OkButton = null;
		private ExtendText m_E_OkTextExtendText = null;
		private Button m_E_CloseButton = null;
		private ExtendImage m_E_CloseExtendImage = null;
		public Transform uiTransform = null;
	}
}
