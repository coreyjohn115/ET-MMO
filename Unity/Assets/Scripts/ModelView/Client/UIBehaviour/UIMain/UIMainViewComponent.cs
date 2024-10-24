using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class UIMainViewComponent : Entity, IAwake, IDestroy 
	{
		public Transform UiTransform => this.uiTransform;

		public LoopVerticalScrollRect E_BottomMenuListLoopVerticalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_BottomMenuListLoopVerticalScrollRect == null )
     			{
		    		this.m_E_BottomMenuListLoopVerticalScrollRect = UIFindHelper.FindDeepChild<LoopVerticalScrollRect>(this.uiTransform.gameObject,"E_BottomMenuList");
     			}
     			return this.m_E_BottomMenuListLoopVerticalScrollRect;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_BottomMenuListLoopVerticalScrollRect = null;
			uiTransform = null;
		}

		private LoopVerticalScrollRect m_E_BottomMenuListLoopVerticalScrollRect = null;
		public Transform uiTransform = null;
	}
}
