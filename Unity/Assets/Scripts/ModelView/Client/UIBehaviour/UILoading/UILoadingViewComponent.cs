using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class UILoadingViewComponent : Entity, IAwake, IDestroy 
	{
		public RawImage E_BgRawImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_BgRawImage == null )
     			{
		    		this.m_E_BgRawImage = UIFindHelper.FindDeepChild<RawImage>(this.uiTransform.gameObject,"E_Bg");
     			}
     			return this.m_E_BgRawImage;
     		}
     	}

		public ExtendText E_DescExtendText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_DescExtendText == null )
     			{
		    		this.m_E_DescExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Desc");
     			}
     			return this.m_E_DescExtendText;
     		}
     	}

		public Slider E_SliderSlider
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_SliderSlider == null )
     			{
		    		this.m_E_SliderSlider = UIFindHelper.FindDeepChild<Slider>(this.uiTransform.gameObject,"E_Slider");
     			}
     			return this.m_E_SliderSlider;
     		}
     	}

		public ExtendText E_PctExtendText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_PctExtendText == null )
     			{
		    		this.m_E_PctExtendText = UIFindHelper.FindDeepChild<ExtendText>(this.uiTransform.gameObject,"E_Pct");
     			}
     			return this.m_E_PctExtendText;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_BgRawImage = null;
			this.m_E_DescExtendText = null;
			this.m_E_SliderSlider = null;
			this.m_E_PctExtendText = null;
			uiTransform = null;
		}

		private RawImage m_E_BgRawImage = null;
		private ExtendText m_E_DescExtendText = null;
		private Slider m_E_SliderSlider = null;
		private ExtendText m_E_PctExtendText = null;
		public Transform uiTransform = null;
	}
}
