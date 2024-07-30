using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ChildOf]
	[EnableMethod]
	public partial class ES_Hp : Entity, ET.IAwake<Transform>, IDestroy 
	{
		public Text E_TextText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_TextText == null )
     			{
		    		this.m_E_TextText = UIFindHelper.FindDeepChild<Text>(this.uiTransform.gameObject,"E_Text");
     			}
     			return this.m_E_TextText;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_TextText = null;
			uiTransform = null;
		}

		private Text m_E_TextText = null;
		public Transform uiTransform = null;
	}
}
