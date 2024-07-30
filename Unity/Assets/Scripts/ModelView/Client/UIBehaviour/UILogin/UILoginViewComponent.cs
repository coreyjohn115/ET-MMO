using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class UILoginViewComponent : Entity, IAwake, IDestroy 
	{
		public RectTransform EG_AccountRectTransform
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_EG_AccountRectTransform == null )
     			{
		    		m_EG_AccountRectTransform = UIFindHelper.FindDeepChild<RectTransform>(uiTransform.gameObject,"EG_Account");
     			}

     			return m_EG_AccountRectTransform;
     		}
     	}

		public InputField E_AccountInputInputField
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_E_AccountInputInputField == null )
     			{
		    		m_E_AccountInputInputField = UIFindHelper.FindDeepChild<InputField>(uiTransform.gameObject,"EG_Account/E_AccountInput");
     			}

     			return m_E_AccountInputInputField;
     		}
     	}

		public ExtendImage E_AccountInputExtendImage
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_E_AccountInputExtendImage == null )
     			{
		    		m_E_AccountInputExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(uiTransform.gameObject,"EG_Account/E_AccountInput");
     			}

     			return m_E_AccountInputExtendImage;
     		}
     	}

		public InputField E_PasswordInputInputField
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_E_PasswordInputInputField == null )
     			{
		    		m_E_PasswordInputInputField = UIFindHelper.FindDeepChild<InputField>(uiTransform.gameObject,"EG_Account/E_PasswordInput");
     			}

     			return m_E_PasswordInputInputField;
     		}
     	}

		public ExtendImage E_PasswordInputExtendImage
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_E_PasswordInputExtendImage == null )
     			{
		    		m_E_PasswordInputExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(uiTransform.gameObject,"EG_Account/E_PasswordInput");
     			}

     			return m_E_PasswordInputExtendImage;
     		}
     	}

		public Button E_LoginBtnButton
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_E_LoginBtnButton == null )
     			{
		    		m_E_LoginBtnButton = UIFindHelper.FindDeepChild<Button>(uiTransform.gameObject,"EG_Account/E_LoginBtn");
     			}

     			return m_E_LoginBtnButton;
     		}
     	}

		public ExtendImage E_LoginBtnExtendImage
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_E_LoginBtnExtendImage == null )
     			{
		    		m_E_LoginBtnExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(uiTransform.gameObject,"EG_Account/E_LoginBtn");
     			}

     			return m_E_LoginBtnExtendImage;
     		}
     	}

		public RectTransform EG_ServerRectTransform
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_EG_ServerRectTransform == null )
     			{
		    		m_EG_ServerRectTransform = UIFindHelper.FindDeepChild<RectTransform>(uiTransform.gameObject,"EG_Server");
     			}

     			return m_EG_ServerRectTransform;
     		}
     	}

		public Button E_ServerBtnButton
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_E_ServerBtnButton == null )
     			{
		    		m_E_ServerBtnButton = UIFindHelper.FindDeepChild<Button>(uiTransform.gameObject,"EG_Server/E_ServerBtn");
     			}

     			return m_E_ServerBtnButton;
     		}
     	}

		public ExtendImage E_ServerBtnExtendImage
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_E_ServerBtnExtendImage == null )
     			{
		    		m_E_ServerBtnExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(uiTransform.gameObject,"EG_Server/E_ServerBtn");
     			}

     			return m_E_ServerBtnExtendImage;
     		}
     	}

		public ExtendText E_ServerTxtExtendText
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_E_ServerTxtExtendText == null )
     			{
		    		m_E_ServerTxtExtendText = UIFindHelper.FindDeepChild<ExtendText>(uiTransform.gameObject,"EG_Server/E_ServerTxt");
     			}

     			return m_E_ServerTxtExtendText;
     		}
     	}

		public Button E_EnterGameBtnButton
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_E_EnterGameBtnButton == null )
     			{
		    		m_E_EnterGameBtnButton = UIFindHelper.FindDeepChild<Button>(uiTransform.gameObject,"EG_Server/E_EnterGameBtn");
     			}

     			return m_E_EnterGameBtnButton;
     		}
     	}

		public ExtendImage E_EnterGameBtnExtendImage
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_E_EnterGameBtnExtendImage == null )
     			{
		    		m_E_EnterGameBtnExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(uiTransform.gameObject,"EG_Server/E_EnterGameBtn");
     			}

     			return m_E_EnterGameBtnExtendImage;
     		}
     	}

		public Button E_BackBtnButton
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_E_BackBtnButton == null )
     			{
		    		m_E_BackBtnButton = UIFindHelper.FindDeepChild<Button>(uiTransform.gameObject,"EG_Server/E_BackBtn");
     			}

     			return m_E_BackBtnButton;
     		}
     	}

		public ExtendImage E_BackBtnExtendImage
     	{
     		get
     		{
     			if (uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}

     			if( m_E_BackBtnExtendImage == null )
     			{
		    		m_E_BackBtnExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(uiTransform.gameObject,"EG_Server/E_BackBtn");
     			}

     			return m_E_BackBtnExtendImage;
     		}
     	}

		public void DestroyWidget()
		{
			m_EG_AccountRectTransform = null;
			m_E_AccountInputInputField = null;
			m_E_AccountInputExtendImage = null;
			m_E_PasswordInputInputField = null;
			m_E_PasswordInputExtendImage = null;
			m_E_LoginBtnButton = null;
			m_E_LoginBtnExtendImage = null;
			m_EG_ServerRectTransform = null;
			m_E_ServerBtnButton = null;
			m_E_ServerBtnExtendImage = null;
			m_E_ServerTxtExtendText = null;
			m_E_EnterGameBtnButton = null;
			m_E_EnterGameBtnExtendImage = null;
			m_E_BackBtnButton = null;
			m_E_BackBtnExtendImage = null;
			uiTransform = null;
		}

		private RectTransform m_EG_AccountRectTransform = null;
		private InputField m_E_AccountInputInputField = null;
		private ExtendImage m_E_AccountInputExtendImage = null;
		private InputField m_E_PasswordInputInputField = null;
		private ExtendImage m_E_PasswordInputExtendImage = null;
		private Button m_E_LoginBtnButton = null;
		private ExtendImage m_E_LoginBtnExtendImage = null;
		private RectTransform m_EG_ServerRectTransform = null;
		private Button m_E_ServerBtnButton = null;
		private ExtendImage m_E_ServerBtnExtendImage = null;
		private ExtendText m_E_ServerTxtExtendText = null;
		private Button m_E_EnterGameBtnButton = null;
		private ExtendImage m_E_EnterGameBtnExtendImage = null;
		private Button m_E_BackBtnButton = null;
		private ExtendImage m_E_BackBtnExtendImage = null;
		public Transform uiTransform = null;
	}
}
