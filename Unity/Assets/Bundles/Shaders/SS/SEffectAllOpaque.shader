Shader "Blz/Effect/AllOpaque"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "gray" {}
		[HDR]_Color ("Color", Color) = (1, 1, 1, 1)

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
		_RimLength("RimLength", Range(0, 10)) = 0

		_Cutoff ("Cutoff", Range(0, 1)) = 0.5

		[HideInInspector]_Mode ("__mode", Float) = 0
		[HideInInspector]_SrcBlend ("__src", Float) = 1
		[HideInInspector]_DstBlend ("__dst", Float) = 0
		[HideInInspector]_ZWrite ("__zw", Float) = 1
		
		[HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
		[HideInInspector]_Stencil("Stencil ID", Float) = 0
		[HideInInspector]_StencilOp("Stencil Operation", Float) = 0
		[HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
		[HideInInspector]_ColorMask("Color Mask", Float) = 15
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Transparent+10" }

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
			ZWrite On

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ IllumMap_On
			#pragma multi_compile _ MaskMap_On
			#pragma multi_compile _ Breath_On
			#pragma multi_compile _ AlphaTest_On AlphaBlend_On
			#pragma multi_compile _ UI_On
			//#pragma target 3.0
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"

			inline float Pow_Schlick(float buttom, float num)
			{
				return buttom / (num - ((num - 1) * buttom));
			}

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 rimcol : COLOR;
				float4 vertex : SV_POSITION;
				float4 args : TEXCOORD2;

				#if UI_On
				float2 worldPos : TEXCOORD3;
				#endif
			};

			sampler2D _MainTex, _MaskTex, _IllumTex, _Glittering;
			float4 _MainTex_ST, _Glittering_ST;
			float _IllumBreath, _Atten, _Cutoff, _MinAlpha, _MaxAlpha, _SpeedX, _SpeedY, _RimLength;
			float4 _Color, _GlitterColor, _IllumColor, _OutLineColor;
			uniform float _FixedTime;

			#if UI_On
			float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
			float2 _ClipArgs0 = float2(1000.0, 1000.0);
			#endif

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv, _Glittering);

				float time_1;
				#if defined(SHADER_API_MOBILE)
				time_1 = _FixedTime * 0.05;
				#else
				time_1 = _Time.x;
				#endif

				float3 viewdir = ObjSpaceViewDir(v.vertex);
				float3 normdir = v.normal;
				float rim = 1 - saturate(dot(normalize(viewdir), normalize(normdir)));
				rim = Pow_Schlick(rim, _Atten);
				o.args = _GlitterColor;

				#if !MaskMap_On
				o.args *= smoothstep(0, 1, rim);
				#endif

				rim = smoothstep(0, _RimLength, rim);
				o.rimcol = rim * _OutLineColor;

				#if IllumMap_On
				#if Breath_On
				o.args.w = (sin(time_1 * 20 * _IllumBreath + 0.1 * o.vertex.x) + 1) * (_MaxAlpha - _MinAlpha) + _MinAlpha;
				#else
				o.args.w = 1;
				#endif
				#endif

				o.uv2 += time_1 * float2(_SpeedX, _SpeedY);

				#if UI_On
				float4 pos = mul(unity_ObjectToWorld, v.vertex);
				o.worldPos = pos.xy * _ClipRange0.zw + _ClipRange0.xy;
				#endif
				return o;
			}


			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv) * _Color;
				float4 glittering = tex2D(_Glittering, i.uv2);

				#if AlphaTest_On
				clip(col.a - _Cutoff);
				#endif

				#if IllumMap_On
				float4 illumcol = tex2D(_IllumTex, i.uv) * _IllumColor;
				#if Breath_On
				illumcol *= i.args.w;
				#endif
				col.rgb += illumcol.rgb;
				#endif

				#if MaskMap_On
				float4 mask = tex2D(_MaskTex, i.uv);
				col.rgb += glittering.rgb * mask.r;
				#else
				col.rgb += glittering.rgb * i.args.xyz;
				#endif

				col = col + i.rimcol;

				#if UI_On
				float2 factor = (float2(1.0, 1.0) - abs(i.worldPos)) * _ClipArgs0;
				col.a *= clamp(min(factor.x, factor.y), 0.0, 1.0);
				#endif

				return col;
			}
			ENDCG
		}
	}

	FallBack "Unlit/Texture"
	CustomEditor "Blz_EffectAllOpaque_GUI"
}
