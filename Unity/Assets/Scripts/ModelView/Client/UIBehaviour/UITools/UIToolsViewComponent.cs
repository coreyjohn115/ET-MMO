using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class UIToolsViewComponent : Entity, IAwake, IDestroy 
	{
		public Transform UiTransform => this.uiTransform;

		public Button E_GmButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_GmButton == null )
     			{
		    		this.m_E_GmButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"E_Gm");
     			}
     			return this.m_E_GmButton;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_GmButton = null;
			uiTransform = null;
		}

		private Button m_E_GmButton = null;
		public Transform uiTransform = null;
	}
}
