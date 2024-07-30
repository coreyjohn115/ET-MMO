using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ChildOf]
	[EnableMethod]
	public partial class ES_OtherImage : Entity, ET.IAwake<Transform>, IDestroy 
	{
		public ES_OtherChatLite ES_OtherChatLite
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_es_otherchatlite == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"ES_OtherChatLite");
		    	   this.m_es_otherchatlite = this.AddChild<ES_OtherChatLite,Transform>(subTrans);
     			}
     			return this.m_es_otherchatlite;
     		}
     	}

		public ExtendImage E_IconExtendImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_IconExtendImage == null )
     			{
		    		this.m_E_IconExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"E_Icon");
     			}
     			return this.m_E_IconExtendImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_es_otherchatlite = null;
			this.m_E_IconExtendImage = null;
			uiTransform = null;
		}

		private EntityRef<ES_OtherChatLite> m_es_otherchatlite = null;
		private ExtendImage m_E_IconExtendImage = null;
		public Transform uiTransform = null;
	}
}
