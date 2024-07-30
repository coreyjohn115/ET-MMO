using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ChildOf]
	[EnableMethod]
	public partial class ES_ComAttr : Entity, IAwake<Transform>, IDestroy 
	{
		public LoopHorizontalScrollRect E_MenuListLoopHorizontalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_MenuListLoopHorizontalScrollRect == null )
     			{
		    		this.m_E_MenuListLoopHorizontalScrollRect = UIFindHelper.FindDeepChild<LoopHorizontalScrollRect>(this.uiTransform.gameObject,"E_MenuList");
     			}
     			return this.m_E_MenuListLoopHorizontalScrollRect;
     		}
     	}

		public LoopHorizontalScrollRect E_ItemGridLoopHorizontalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ItemGridLoopHorizontalScrollRect == null )
     			{
		    		this.m_E_ItemGridLoopHorizontalScrollRect = UIFindHelper.FindDeepChild<LoopHorizontalScrollRect>(this.uiTransform.gameObject,"E_ItemGrid");
     			}
     			return this.m_E_ItemGridLoopHorizontalScrollRect;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_MenuListLoopHorizontalScrollRect = null;
			this.m_E_ItemGridLoopHorizontalScrollRect = null;
			uiTransform = null;
		}

		private LoopHorizontalScrollRect m_E_MenuListLoopHorizontalScrollRect = null;
		private LoopHorizontalScrollRect m_E_ItemGridLoopHorizontalScrollRect = null;
		public Transform uiTransform = null;
	}
}
