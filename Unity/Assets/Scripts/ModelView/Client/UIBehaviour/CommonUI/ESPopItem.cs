using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ChildOf]
	[EnableMethod]
	public partial class ESPopItem : Entity, ET.IAwake<Transform>, IDestroy 
	{
		public RectTransform EG_ContentRectTransform
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EG_ContentRectTransform == null )
     			{
		    		this.m_EG_ContentRectTransform = UIFindHelper.FindDeepChild<RectTransform>(this.uiTransform.gameObject,"EG_Content");
     			}
     			return this.m_EG_ContentRectTransform;
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
     			if( this.m_E_TextExtendText == null )
     			{
		    		this.m_E_TextExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"EG_Content/E_Text");
     			}
     			return this.m_E_TextExtendText;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EG_ContentRectTransform = null;
			this.m_E_TextExtendText = null;
			uiTransform = null;
		}

		private RectTransform m_EG_ContentRectTransform = null;
		private ExtendText m_E_TextExtendText = null;
		public Transform uiTransform = null;
	}
}
