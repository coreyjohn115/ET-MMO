using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ChildOf]
	[EnableMethod]
	public partial class ESItem : Entity, IAwake<Transform>, IDestroy 
	{
		public Button E_ContentButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ContentButton == null )
     			{
		    		this.m_E_ContentButton = UIFindHelper.FindDeepChild<Button>(this.uiTransform.gameObject,"E_Content");
     			}
     			return this.m_E_ContentButton;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_ContentButton = null;
			uiTransform = null;
		}

		private Button m_E_ContentButton = null;
		public Transform uiTransform = null;
	}
}
