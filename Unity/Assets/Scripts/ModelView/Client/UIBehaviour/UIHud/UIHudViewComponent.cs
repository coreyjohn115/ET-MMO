using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class UIHudViewComponent : Entity, IAwake, IDestroy 
	{
		public RectTransform EG_SelfRectTransform
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EG_SelfRectTransform == null )
     			{
		    		this.m_EG_SelfRectTransform = UIFindHelper.FindDeepChild<RectTransform>(this.uiTransform.gameObject,"EG_Self");
     			}
     			return this.m_EG_SelfRectTransform;
     		}
     	}

		public RectTransform EG_OtherRectTransform
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EG_OtherRectTransform == null )
     			{
		    		this.m_EG_OtherRectTransform = UIFindHelper.FindDeepChild<RectTransform>(this.uiTransform.gameObject,"EG_Other");
     			}
     			return this.m_EG_OtherRectTransform;
     		}
     	}

		public RectTransform EG_DropRectTransform
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EG_DropRectTransform == null )
     			{
		    		this.m_EG_DropRectTransform = UIFindHelper.FindDeepChild<RectTransform>(this.uiTransform.gameObject,"EG_Drop");
     			}
     			return this.m_EG_DropRectTransform;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EG_SelfRectTransform = null;
			this.m_EG_OtherRectTransform = null;
			this.m_EG_DropRectTransform = null;
			uiTransform = null;
		}

		private RectTransform m_EG_SelfRectTransform = null;
		private RectTransform m_EG_OtherRectTransform = null;
		private RectTransform m_EG_DropRectTransform = null;
		public Transform uiTransform = null;
	}
}
