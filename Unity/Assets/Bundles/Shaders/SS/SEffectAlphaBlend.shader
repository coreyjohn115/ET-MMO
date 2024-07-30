Shader "Blz/Effect/AlphaBlend" {
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[HDR]_Color ("Color", Color) = (1, 1, 1, 1)
		_Intensity ("Intensity", Float) = 1
		_ZTest("ZTest", Float) = 4

		[HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
		[HideInInspector]_Stencil("Stencil ID", Float) = 0
		[HideInInspector]_StencilOp("Stencil Operation", Float) = 0
		[HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
		[HideInInspector]_ColorMask("Color Mask", Float) = 15
	}
	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZTest [_ZTest]
		ZWrite Off
		SubShader
		{
			Pass
			{
				Stencil
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
				#pragma fragmentoption ARB_precision_hint_fastest
				
				#include "UnityCG.cginc"
				#include "UnityUI.cginc"

				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					float4 color : COLOR;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _Color;
				float _Intensity;
				float4 _ClipRect;

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 texcoord : TEXCOORD0;
					float4 color : COLOR;

					#if UNITY_UI_CLIP_RECT
					float2 objPos : TEXCOORD1;
					#endif
				};

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color * _Color * _Intensity;
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

					#if UNITY_UI_CLIP_RECT
					o.objPos = v.vertex;
					#endif
					return o;
				}


				float4 frag (v2f i) : SV_Target
				{
					float4 col = tex2D(_MainTex, i.texcoord);
					col *= i.color;

					#if UNITY_UI_CLIP_RECT
					col.a *= UnityGet2DClipping(i.objPos.xy, _ClipRect);
					#endif

					return col;
				}
				ENDCG
			}
		}
	}
}