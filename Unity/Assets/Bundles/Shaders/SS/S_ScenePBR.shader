Shader "Blz/Scene/PBR"
{
	Properties
	{
		_Color("Color",color) = (1,1,1,1)	
		_MainTex("Main Tex",2D) = "white"{}	

		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		_MetallicGlossMap("Metallic",2D) = "white"{}
		_Smoothness("Smoothness",Range(0,1)) = 0.5 

		_BumpMap("Normal Map",2D) = "bump"{}
		_BumpScale("Normal Scale",float) = 1 

		_EmissMap ("Emiss Map", 2D) = "white" {}
		[HDR]_EmissColor ("Emiss Color", Color) = (1, 1, 1, 1)
		_EmissMaskSpeedX ("Emiss Mask Speed X", float) = 0
		_EmissMaskSpeedY ("Emiss Mask Speed Y", float) = 0
		_EmissIntensity ("Emiss Intensity", float) = 5		
		_EmissNoiseFactor ("Emiss Noise Factor", float) = 0
		_EmissNoiseStrength ("Emiss Noise Strength", float) = 0
		_EmissBreathSpeed ("Emiss Breath Speed", float) = 0
        _EmissBreathMap("Emiss Breath Map", 2D) = "white" {}

		_OutlineColor("OutlineColor", Color) = (1, 1, 1, 1)
		_OutlineLength("OutlineLength", float) = 0.015
		_OutlineLightness ("OutlineLightness", Range(0, 1)) = 0.5
		_OutlineCameraVector("OutlineCameraVector", float) = 0.01
		_OutlineType("OutlineType", float) = 0

		[HideInInspector] _Mode ("__mode", Float) = 0
	}
	
	SubShader
	{
		Tags{"RenderType" = "Opaque" "PerformanceChecks"="False"}

		Pass
        {
			Name "OUTLINE"
			Tags{"LightMode" = "Deferred"}
			Cull Front
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert1
			#pragma fragment frag1
			#pragma multi_compile_fog
			#pragma multi_compile_instancing
			#pragma multi_compile _ AlphaTest_ON

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0

			fixed4 _OutlineColor;
			float _OutlineLightness;
			float _OutlineLength;
			float _IsUI;
			float _OutlineCameraVector;
			float _OutlineType;

			struct VertexInput {
				float4 vertex : POSITION;
				float4 color : TEXCOORD4;
				float4 normal : NORMAL;
				float2 texcoord0 : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			struct VertexOutput {
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				UNITY_FOG_COORDS(1)

				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#include "Blz_CG.cginc"


			VertexOutput vert1(VertexInput v) {
				VertexOutput o ;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.uv0 = v.texcoord0;
				o.pos = UnityObjectToClipPos(v.vertex);
				float4 vPos = float4(UnityObjectToViewPos(v.vertex),1.0f);
				float cameraDis = length(vPos.xyz);
				float3 n = v.normal.xyz * (1 - _OutlineType) + v.color.xyz * _OutlineType;
				float3 Vnormal = mul(UNITY_MATRIX_IT_MV, n);
				float2 offse1t = normalize(TransformViewToProjection(Vnormal.xy)) * 2;
				if (UNITY_MATRIX_P[3][3] == 0)
					offse1t += offse1t * cameraDis * _OutlineCameraVector;
				o.pos.xy += offse1t*_OutlineLength;
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			void frag1(VertexOutput i, out half4 outGBuffer0 : SV_Target0, out half4 outGBuffer1 : SV_Target1,
					out half4 outGBuffer2 : SV_Target2, out half4 outEmission : SV_Target3)
			{
				UNITY_SETUP_INSTANCE_ID(i);

				outGBuffer0 = 1;outGBuffer1 = 1;outGBuffer2 = 0;outEmission = 0;
				float4 col = tex2D(_MainTex, i.uv0);
				#if defined(AlphaTest_ON)
					clip (col.a - _Cutoff);
				#endif
				ToGBufferData data;
				data.diffuseColor   = fixed4(0, 0, 0, 1);
				data.occlusion      = 0;
				data.specularColor  = 0;
				data.smoothness     = 0;
				data.normalWorld    = 0;

				ComputeToGbuffer(data, outGBuffer0, outGBuffer1, outGBuffer2);

				outEmission = fixed4(0, 0, 0, 1);
			}
			ENDCG
        }

		pass
		{
			Tags{"LightMode" = "ForwardBase"}
			CGPROGRAM

			#pragma target 3.0
			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog
			#pragma multi_compile_instancing
			#pragma multi_compile _ EmissMap_ON
			#pragma multi_compile _ EmissBreathMap_ON
			#pragma multi_compile _ MetallicMap_ON
			#pragma multi_compile _ AlphaTest_ON

			#pragma vertex vertForwardBaseAdvanced
			#pragma fragment fragForwardBaseAdvanced

			#include "Blz_CG.cginc"

			ENDCG
		}

		pass
		{
			Tags{"LightMode" = "Deferred"}
			CGPROGRAM

			#pragma target 3.0
            #pragma exclude_renderers nomrt

			#pragma multi_compile _ EmissMap_ON
			#pragma multi_compile _ EmissBreathMap_ON
			#pragma multi_compile _ MetallicMap_ON
			#pragma multi_compile _ AlphaTest_ON

			#pragma vertex vertDeferredAdvanced
			#pragma fragment fragDeferredAdvanced

			#pragma multi_compile_prepassfinal
            #pragma multi_compile_instancing

			#include "Blz_CG.cginc"

			ENDCG
		}
	}
	FallBack "VertexLit"
	CustomEditor "S_ScenePBR_GUI"
}
