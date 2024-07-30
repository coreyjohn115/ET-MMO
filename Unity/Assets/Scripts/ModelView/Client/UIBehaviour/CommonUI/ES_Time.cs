using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[ChildOf]
	[EnableMethod]
	public partial class ES_Time : Entity, ET.IAwake<Transform>, IDestroy 
	{
		public ExtendText E_TimeExtendText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_TimeExtendText == null )
     			{
		    		this.m_E_TimeExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Time");
     			}
     			return this.m_E_TimeExtendText;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_TimeExtendText = null;
			uiTransform = null;
		}

		private ExtendText m_E_TimeExtendText = null;
		public Transform uiTransform = null;
	}
}
