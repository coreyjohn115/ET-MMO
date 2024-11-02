Shader "UI/ImageGrey"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}//UI Image上选择的图片
		_Color ("Tint", Color) = (1,1,1,1)
		//_Blend ("Blend", Range(0,1)) = 1
		//[Toggle] _IsAnimate ("Is Animate", Float) = 0
		//_Speed ("Speed", float) = 2.5
		//[Space(20)]

		// Required for UI.Mask
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
	}
	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		// 源rgba*源a + 背景rgba*(1-源A值)
		Blend SrcAlpha OneMinusSrcAlpha
		ZTest [unity_GUIZTestMode]
		ZWrite Off

		Pass
		{
			Stencil//要支持Mask必须，否则在Mask下不会被裁剪
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }
            ColorMask[_ColorMask]

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile __ UNITY_UI_CLIP_RECT

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				//float2 texcoord1 : TEXCOORD1;
				//float2 texcoord2 : TEXCOORD2;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//float2 uv1 : TEXCOORD1;
				//float4 tangent: TEXCOORD3;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float4 objPos : TEXCOORD2;
			};

			fixed4 _Color;
			float _Blend;
			bool _IsAnimate;
			float _Speed;
			float _ExtandUI;		
			float4 _ClipRect;
			float4 _UvRect;

			v2f vert (appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				o.objPos = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);//转换坐标空间
				o.uv = v.texcoord;
				//o.uv1 = v.texcoord1;
				//o.tangent.xy = v.texcoord2;
				//#ifdef UNITY_HALF_TEXEL_OFFSET
				//o.vertex.xy -= (_ScreenParams.zw-1.0);
				//#endif
				o.color = v.color*_Color;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 color = tex2D(_MainTex, i.uv) * i.color;
				fixed3 grey = dot(color.rgb, fixed3(0.22, 0.707, 0.071));
				//if (_ExtandUI == 1 && i.tangent.x == 1) {
					//float blend = _Blend*(!_IsAnimate)+abs(sin(_Time.y*_Speed))*_IsAnimate;
					//color.rgb = lerp(color.rgb, grey, blend);					
					color.rgb = grey;				
				//}
				//if(_ExtandUI == 0) color.rgb = grey;					
#ifdef UNITY_UI_CLIP_RECT
				color.a *= UnityGet2DClipping(i.objPos.xy, _ClipRect);
#endif
				//if (i.tangent.y == 1){
				//	float2 tc = float2(0, 0);
				//	tc.x = _UvRect.x + (_UvRect.z - _UvRect.x) * i.uv1.x;
				//	tc.y = _UvRect.y + (_UvRect.w - _UvRect.y) * i.uv1.y;
				//	fixed2 dir = float2(_UvRect.x + (_UvRect.z - _UvRect.x) * 0.5, _UvRect.y + (_UvRect.w - _UvRect.y) * 0.5) - tc;
				//	fixed len = length(dir);
				//	if (len > (_UvRect.z - _UvRect.x) * 0.5) color.a = 0;
				//}
				return color;
			}
			ENDCG
		}
	}
}
