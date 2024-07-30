using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ChildOf]
	[EnableMethod]
	public partial class ES_ComBag : Entity, IAwake<Transform>, IDestroy 
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

		public LoopVerticalScrollRect E_ItemGridLoopVerticalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ItemGridLoopVerticalScrollRect == null )
     			{
		    		this.m_E_ItemGridLoopVerticalScrollRect = UIFindHelper.FindDeepChild<LoopVerticalScrollRect>(this.uiTransform.gameObject,"E_ItemGrid");
     			}
     			return this.m_E_ItemGridLoopVerticalScrollRect;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_MenuListLoopHorizontalScrollRect = null;
			this.m_E_ItemGridLoopVerticalScrollRect = null;
			uiTransform = null;
		}

		private LoopHorizontalScrollRect m_E_MenuListLoopHorizontalScrollRect = null;
		private LoopVerticalScrollRect m_E_ItemGridLoopVerticalScrollRect = null;
		public Transform uiTransform = null;
	}
}
