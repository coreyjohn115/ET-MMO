Shader "Blz/Effect/AllTransparent"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "gray" {}
		[HDR]_Color ("Color", Color) = (1, 1, 1, 1)
		_RotateSpeed("RotateSpeed", Float) = 0
		_FloatSpeedX("FloatSpeedX", Float) = 0
		_FloatSpeedY("FloatSpeedY", Float) = 0

		_IllumTex ("IllumMap", 2D) = "black" {}
		[HDR]_IllumColor ("IllumColor", Color) = (1, 1, 1, 1)
		_IllumBreath ("IllumBreath", Range(0, 10)) = 0
		_MinAlpha ("MinAlpha", Float) = 0
		_MaxAlpha ("MaxAlpha", Float) = 0

		_Glittering ("Glittering", 2D) = "black" {}
		[HDR]_GlitterColor ("GlitterColor", Color) = (1, 1, 1, 1)
		_SpeedX ("SpeedX", Float) = 0
		_SpeedY ("SpeedY", Float) = 0

		_MaskTex ("MaskTex", 2D) = "white" {}
		_Atten ("Atten", Range(0, 5)) = 0

		[HDR]_OutLineColor("OutLineColor", Color) = (0, 0, 0, 0)
		_RimLength("RimLength", Range(0, 1)) = 0
		_Intensity ("Intensity", Range(0, 10)) = 1

		[HideInInspector]_Mode ("__mode", Float) = 0
		[HideInInspector]_SrcBlend ("__src", Float) = 1
		[HideInInspector]_DstBlend ("__dst", Float) = 0

		[HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
		[HideInInspector]_Stencil("Stencil ID", Float) = 0
		[HideInInspector]_StencilOp("Stencil Operation", Float) = 0
		[HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
		[HideInInspector]_ColorMask("Color Mask", Float) = 15
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent+10" }

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

			Blend [_SrcBlend] [_DstBlend]
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ MainTex_On
			#pragma multi_compile _ Rotate_On
			#pragma multi_compile _ IllumMap_On
			#pragma multi_compile _ MaskMap_On
			#pragma multi_compile _ Breath_On
			#pragma multi_compile _ Atten_On
			#pragma multi_compile _ HDR
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile __ UNITY_UI_CLIP_RECT

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			inline float Pow_Schlick(float buttom, float num)
			{
				return buttom / (num - ((num - 1) * buttom));
			}

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 col : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float4 vertex : SV_POSITION;
				float4 args : TEXCOORD3;

				#if MaskMap_On
				float2 uvmask : TEXCOORD4;
				#endif

				#if Atten_On
				float4 rimcol : TEXCOORD5;
				#endif

				#if UNITY_UI_CLIP_RECT
				float2 objPos : TEXCOORD6;
				#endif
				float4 col : COLOR;
			};

			sampler2D _MainTex, _MaskTex, _IllumTex, _Glittering;
			float4 _MainTex_ST, _IllumTex_ST, _Glittering_ST;
			float _IllumBreath, _Atten, _MinAlpha, _MaxAlpha, _SpeedX, _SpeedY, _RimLength, _RotateSpeed, _FloatSpeedX, _FloatSpeedY;
			float4 _Color, _GlitterColor, _IllumColor, _OutLineColor;
			float4 _ClipRect;
			uniform float _FixedTime;
			uniform float _FixedCircleTime;

			v2f vert (appdata v)
			{
				v2f o=(v2f)0;
				#if UNITY_UI_CLIP_RECT
				o.objPos = v.vertex;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				float time_1, time_2;
				//#if defined(SHADER_API_MOBILE)
				//time_1 = _FixedTime * 0.05;
				//time_2 = _FixedCircleTime * 0.05;
				//#else
				time_1 = _Time.x;
				time_2 = _Time.x;
				//#endif

				#if Rotate_On
				float2 rotate = float2(cos(time_2 * _RotateSpeed), sin(time_2 * _RotateSpeed));
				float2 uv = o.uv - float2(0.5, 0.5);
				uv = float2((uv.x * rotate.x - uv.y * rotate.y), (uv.x * rotate.y + uv.y * rotate.x));
				o.uv = uv + float2(0.5, 0.5);
				#else
				o.uv += time_1 * float2(_FloatSpeedX, _FloatSpeedY);
				#endif

				o.uv2 = TRANSFORM_TEX(v.uv, _Glittering);
				#if MaskMap_On
				o.uvmask = v.uv;
				#endif

				o.args = _GlitterColor;

				#if Atten_On
				float3 viewdir = ObjSpaceViewDir(v.vertex);
				float rim = 1 - saturate(dot(normalize(viewdir), normalize(v.normal)));
				float finalrim = smoothstep(1 - _RimLength, 1, rim);
				o.rimcol = finalrim * _OutLineColor;
				rim = Pow_Schlick(rim, _Atten);
				o.args *= smoothstep(0, 1, rim);
				#endif

				#if IllumMap_On
				o.uv1 = TRANSFORM_TEX(v.uv, _IllumTex);
				#if Breath_On
				o.args.w = (sin(time_2 * 20 * _IllumBreath + 0.1 * o.vertex.x) + 1) * (_MaxAlpha - _MinAlpha) + _MinAlpha;
				#else
				o.args.w = 1;
				#endif
				#endif

				o.uv2 += time_1 * float2(_SpeedX, _SpeedY);
				o.col = v.col;

				return o;
			}

			float _Intensity;
			float4 frag (v2f i) : SV_Target
			{
				float4 col = float4(0, 0, 0, 0.5);
				#if MainTex_On
				col = tex2D(_MainTex, i.uv) * _Color;
				#endif

				float4 glittering = tex2D(_Glittering, i.uv2);

				#if IllumMap_On
				float4 illumcol = tex2D(_IllumTex, i.uv1) * _IllumColor;
				#if Breath_On
				illumcol *= i.args.w;
				#endif
				col.rgb += illumcol.rgb;
				#endif

				col.rgb += glittering.rgb * i.args.xyz;
				#if MaskMap_On
				float4 mask = tex2D(_MaskTex, i.uvmask);
				col.a *= mask.r;
				#endif

				#if Atten_On
				col += i.rimcol;
				#endif

				col = col * i.col;

				#if UNITY_UI_CLIP_RECT
				col.a *= UnityGet2DClipping(i.objPos.xy, _ClipRect);
				#endif

				return col;
			}
			ENDCG
		}
	}

	FallBack "Unlit/Texture"
	CustomEditor "Blz_EffectAllTransparent_GUI"
}
