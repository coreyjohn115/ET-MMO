using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ChildOf]
	[EnableMethod]
	public partial class ES_SelfText : Entity, ET.IAwake<Transform>, IDestroy 
	{
		public ES_SelfChatLite ES_ChatLite
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_es_chatlite == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"ES_ChatLite");
		    	   this.m_es_chatlite = this.AddChild<ES_SelfChatLite,Transform>(subTrans);
     			}
     			return this.m_es_chatlite;
     		}
     	}

		public ExtendImage E_BgExtendImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_BgExtendImage == null )
     			{
		    		this.m_E_BgExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"E_Bg");
     			}
     			return this.m_E_BgExtendImage;
     		}
     	}

		public LongClickButton E_MsgLongClickButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_MsgLongClickButton == null )
     			{
		    		this.m_E_MsgLongClickButton = UIFindHelper.FindDeepChild<LongClickButton>(this.uiTransform.gameObject,"E_Bg/E_Msg");
     			}
     			return this.m_E_MsgLongClickButton;
     		}
     	}

		public SymbolText E_MsgSymbolText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_MsgSymbolText == null )
     			{
		    		this.m_E_MsgSymbolText = UIFindHelper.FindDeepChild<SymbolText>(this.uiTransform.gameObject,"E_Bg/E_Msg");
     			}
     			return this.m_E_MsgSymbolText;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_es_chatlite = null;
			this.m_E_BgExtendImage = null;
			this.m_E_MsgLongClickButton = null;
			this.m_E_MsgSymbolText = null;
			uiTransform = null;
		}

		private EntityRef<ES_SelfChatLite> m_es_chatlite = null;
		private ExtendImage m_E_BgExtendImage = null;
		private LongClickButton m_E_MsgLongClickButton = null;
		private SymbolText m_E_MsgSymbolText = null;
		public Transform uiTransform = null;
	}
}
