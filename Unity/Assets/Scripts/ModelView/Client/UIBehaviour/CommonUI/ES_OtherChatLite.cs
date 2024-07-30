using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ChildOf]
	[EnableMethod]
	public partial class ES_OtherChatLite : Entity, ET.IAwake<Transform>, IDestroy 
	{
		public ES_PlayerLite ES_PlayerLite
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_es_playerlite == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"ES_PlayerLite");
		    	   this.m_es_playerlite = this.AddChild<ES_PlayerLite,Transform>(subTrans);
     			}
     			return this.m_es_playerlite;
     		}
     	}

		public ExtendImage E_SexExtendImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_SexExtendImage == null )
     			{
		    		this.m_E_SexExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"Title/E_Sex");
     			}
     			return this.m_E_SexExtendImage;
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
		    		this.m_E_LevelExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"Title/E_Level");
     			}
     			return this.m_E_LevelExtendText;
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
		    		this.m_E_NameExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"Title/E_Name");
     			}
     			return this.m_E_NameExtendText;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_es_playerlite = null;
			this.m_E_SexExtendImage = null;
			this.m_E_LevelExtendText = null;
			this.m_E_NameExtendText = null;
			uiTransform = null;
		}

		private EntityRef<ES_PlayerLite> m_es_playerlite = null;
		private ExtendImage m_E_SexExtendImage = null;
		private ExtendText m_E_LevelExtendText = null;
		private ExtendText m_E_NameExtendText = null;
		public Transform uiTransform = null;
	}
}
