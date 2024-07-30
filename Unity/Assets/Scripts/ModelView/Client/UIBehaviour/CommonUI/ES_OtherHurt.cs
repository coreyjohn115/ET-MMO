using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ChildOf]
	[EnableMethod]
	public partial class ES_OtherHurt : Entity, IAwake<Transform>, IDestroy 
	{
		public Text E_TextText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_TextText == null )
     			{
		    		this.m_E_TextText = UIFindHelper.FindDeepChild<Text>(this.uiTransform.gameObject,"E_Text");
     			}
     			return this.m_E_TextText;
     		}
     	}

		public ExtendImage E_ParryExtendImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ParryExtendImage == null )
     			{
		    		this.m_E_ParryExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"E_Text/E_Parry");
     			}
     			return this.m_E_ParryExtendImage;
     		}
     	}

		public ExtendImage E_ReboundExtendImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ReboundExtendImage == null )
     			{
		    		this.m_E_ReboundExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"E_Text/E_Rebound");
     			}
     			return this.m_E_ReboundExtendImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_TextText = null;
			this.m_E_ParryExtendImage = null;
			this.m_E_ReboundExtendImage = null;
			uiTransform = null;
		}

		private Text m_E_TextText = null;
		private ExtendImage m_E_ParryExtendImage = null;
		private ExtendImage m_E_ReboundExtendImage = null;
		public Transform uiTransform = null;
	}
}
