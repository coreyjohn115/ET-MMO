using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ChildOf]
	[EnableMethod]
	public partial class ES_EquipItem : Entity, IAwake<Transform>, IDestroy 
	{
		public ExtendImage E_HeadExtendImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_HeadExtendImage == null )
     			{
		    		this.m_E_HeadExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"E_Head");
     			}
     			return this.m_E_HeadExtendImage;
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
     			if( this.m_E_NameExtendText == null )
     			{
		    		this.m_E_NameExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Head/E_Name");
     			}
     			return this.m_E_NameExtendText;
     		}
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
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"E_Head/ESItem");
		    	   this.m_esitem = this.AddChild<ESItem,Transform>(subTrans);
     			}
     			return this.m_esitem;
     		}
     	}

		public ExtendText E_TypeExtendText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_TypeExtendText == null )
     			{
		    		this.m_E_TypeExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Head/Type/E_Type");
     			}
     			return this.m_E_TypeExtendText;
     		}
     	}

		public ExtendText E_BindExtendText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_BindExtendText == null )
     			{
		    		this.m_E_BindExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Head/E_Bind");
     			}
     			return this.m_E_BindExtendText;
     		}
     	}

		public ExtendText E_LevelExtendText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_LevelExtendText == null )
     			{
		    		this.m_E_LevelExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Head/Level/E_Level");
     			}
     			return this.m_E_LevelExtendText;
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
		    		this.m_E_DescExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"Center/E_Desc");
     			}
     			return this.m_E_DescExtendText;
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
		    		this.m_E_CancelButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"E_Bottom/E_Cancel");
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
		    		this.m_E_CancelTextExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Bottom/E_Cancel/E_CancelText");
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
		    		this.m_E_OkButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"E_Bottom/E_Ok");
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
		    		this.m_E_OkTextExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Bottom/E_Ok/E_OkText");
     			}
     			return this.m_E_OkTextExtendText;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_HeadExtendImage = null;
			this.m_E_NameExtendText = null;
			this.m_esitem = null;
			this.m_E_TypeExtendText = null;
			this.m_E_BindExtendText = null;
			this.m_E_LevelExtendText = null;
			this.m_E_DescExtendText = null;
			this.m_E_CancelButton = null;
			this.m_E_CancelTextExtendText = null;
			this.m_E_OkButton = null;
			this.m_E_OkTextExtendText = null;
			uiTransform = null;
		}

		private ExtendImage m_E_HeadExtendImage = null;
		private ExtendText m_E_NameExtendText = null;
		private EntityRef<ESItem> m_esitem = null;
		private ExtendText m_E_TypeExtendText = null;
		private ExtendText m_E_BindExtendText = null;
		private ExtendText m_E_LevelExtendText = null;
		private ExtendText m_E_DescExtendText = null;
		private Button m_E_CancelButton = null;
		private ExtendText m_E_CancelTextExtendText = null;
		private Button m_E_OkButton = null;
		private ExtendText m_E_OkTextExtendText = null;
		public Transform uiTransform = null;
	}
}
