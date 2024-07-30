using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ChildOf]
	[EnableMethod]
	public partial class ES_PlayerLite : Entity, ET.IAwake<Transform>, IDestroy 
	{
		public Button E_HeadButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_HeadButton == null )
     			{
		    		this.m_E_HeadButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"E_Head");
     			}
     			return this.m_E_HeadButton;
     		}
     	}

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

		public ExtendImage E_FrameExtendImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_FrameExtendImage == null )
     			{
		    		this.m_E_FrameExtendImage = UIFindHelper.FindDeepChild<ExtendImage>(this.uiTransform.gameObject,"E_Frame");
     			}
     			return this.m_E_FrameExtendImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_HeadButton = null;
			this.m_E_HeadExtendImage = null;
			this.m_E_FrameExtendImage = null;
			uiTransform = null;
		}

		private Button m_E_HeadButton = null;
		private ExtendImage m_E_HeadExtendImage = null;
		private ExtendImage m_E_FrameExtendImage = null;
		public Transform uiTransform = null;
	}
}
