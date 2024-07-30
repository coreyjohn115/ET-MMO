Shader "Blz/Scene/Diffuse"
{
    Properties
    {
		[HideInInspector]_DiffuseType ("__DiffuseType", Float) = 0

		_Color ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("Main Tex", 2D) = "white" {}
		//自发光贴图：r通道画自发光纹理，g通道画自发光内部的亮暗扰动纹理，b通道画自发光外部的扰动纹理
		_EmissMap ("Emiss Map", 2D) = "white" {}
		[HDR]_EmissColor ("Emiss Color", Color) = (1, 1, 1, 1)
		_EmissMaskSpeedX ("Emiss Mask Speed X", float) = 0
		_EmissMaskSpeedY ("Emiss Mask Speed Y", float) = 0	
		_EmissNoiseFactor ("Emiss Noise Factor", float) = 0
		_EmissNoiseStrength ("Emiss Noise Strength", float) = 0
		_EmissBreathSpeed ("Emiss Breath Speed", float) = 0
        _EmissBreathMap("Emiss Breath Map", 2D) = "white" {}

		//_BumpMap ("Normal Map", 2D) = "bump"{}
		//_BumpScale ("Bump Scale", float) = 1

        _OutlineColor("OutlineColor", Color) = (1, 1, 1, 1)
		_OutlineLength("OutlineLength", float) = 0.015
		_OutlineLightness ("OutlineLightness", Range(0, 1)) = 0.5
		_OutlineCameraVector("OutlineCameraVector", float) = 0.01
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque"}
        LOD 100 
        
        Pass
        {
			Name "OUTLINE"
			Cull Front
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0

			sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _OutlineColor;
			float _OutlineLightness;
			float _OutlineLength;
			float _IsUI;
			float _OutlineCameraVector;

			struct VertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord0 : TEXCOORD0;
			};
			struct VertexOutput {
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};
			VertexOutput vert(VertexInput v) {
				VertexOutput o = (VertexOutput)0;
				o.uv0 = v.texcoord0;
				o.pos = UnityObjectToClipPos(v.vertex);
				float4 vPos = float4(UnityObjectToViewPos(v.vertex),1.0f);
				float cameraDis = length(vPos.xyz);
				float3 Vnormal = mul(UNITY_MATRIX_IT_MV, v.normal);
				float2 offse1t = normalize(TransformViewToProjection(Vnormal.xy)) * 2;
				if (UNITY_MATRIX_P[3][3] == 0)
					offse1t += offse1t * cameraDis * _OutlineCameraVector;
				o.pos.xy += offse1t*_OutlineLength;
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			float4 frag(VertexOutput i) : COLOR
			{
				float3 col = tex2D(_MainTex, i.uv0);
				float4 finalColor = float4((col*_OutlineLightness * col), 0);
				UNITY_APPLY_FOG(i.fogCoord, finalColor);
				return finalColor;
			}
			ENDCG
        }

        Pass
        {
            Tags { "LightMode"="ForwardBase" }

            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
			#pragma multi_compile _ BumpMap_ON
			#pragma multi_compile _ EmissMap_ON
            #pragma multi_compile _ EmissBreathMap_ON

            #include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"

			float _DiffuseType;
			float4 _Color;
            sampler2D _MainTex; float4 _MainTex_ST;
			sampler2D _EmissMap; float4 _EmissMap_ST;
            sampler2D _EmissBreathMap;float4 _EmissBreathMap_ST;
			float4 _EmissColor;float _EmissMaskSpeedX; float _EmissMaskSpeedY;
			float _EmissNoiseFactor; float _EmissNoiseStrength;float _EmissBreathSpeed;
            sampler2D _BumpMap; fixed4 _BumpMap_ST; fixed _BumpScale;

            struct vertexIN_base
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float4 tangent: TANGENT;
            };
            
            struct v2f_base
            {
                float4 pos : SV_POSITION;
                float3 vertexLight : COLOR;
                float2  uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 lightDir : TEXCOORD2;
                UNITY_FOG_COORDS(3)
				float4 emissInfo : TEXCOORD4;
				SHADOW_COORDS(5)
				float4 TtoW0 : TEXCOORD6; 
				float4 TtoW1 : TEXCOORD7;
				float4 TtoW2 : TEXCOORD8;
                float4 emissBreath : TEXCOORD9;
                #ifdef LIGHTMAP_ON
				float2 texcoordLM : TEXCOORD10;
                #endif
            };

            v2f_base vert(vertexIN_base v)
            {
                v2f_base o;
                o.pos = UnityObjectToClipPos(v.vertex);
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldTangent = UnityObjectToWorldDir(v.tangent);
				float3 worldBinormal = cross(worldNormal,worldTangent) * v.tangent.w;
                o.TtoW0 = float4(worldTangent.x,worldBinormal.x,worldNormal.x,worldPos.x);
				o.TtoW1 = float4(worldTangent.y,worldBinormal.y,worldNormal.y,worldPos.y);
				o.TtoW2 = float4(worldTangent.z,worldBinormal.z,worldNormal.z,worldPos.z);

                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.emissInfo.xy = TRANSFORM_TEX(v.texcoord, _EmissMap).xy;
				o.emissInfo.zw = o.emissInfo.xy + _Time.z * float2(_EmissMaskSpeedX / 100, _EmissMaskSpeedY / 100);
                o.emissBreath.xy = TRANSFORM_TEX(v.texcoord, _EmissBreathMap);
                o.normal = v.normal;
                o.lightDir = ObjSpaceLightDir(v.vertex);

                float3 shlight = ShadeSH9(float4(worldNormal, 1.0));
                o.vertexLight = shlight;
				#ifdef VERTEXLIGHT_ON
                    o.vertexLight += Shade4PointLights (
                        unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
                        unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
                        unity_4LightAtten0, worldPos, worldNormal
                        );
                #endif
                #ifdef LIGHTMAP_ON
				o.texcoordLM = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				TRANSFER_SHADOW(o);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o; 
            }
            
            float4 frag(v2f_base i) : COLOR
            {
                i.lightDir = normalize(i.lightDir);
                i.normal = normalize(i.normal);

				fixed3 b_tangentNormal = UnpackScaleNormal(tex2D(_BumpMap, TRANSFORM_TEX(i.uv, _BumpMap)), _BumpScale);
				fixed3 bump = normalize(float3(dot(i.TtoW0.xyz, b_tangentNormal), dot(i.TtoW1.xyz, b_tangentNormal), dot(i.TtoW2.xyz, b_tangentNormal)));

                float diffuse = max(0, dot(i.normal, i.lightDir));

                float4 mainColor = tex2D(_MainTex, i.uv);
				
				float4 c = mainColor * _Color;
			
				float shadow = SHADOW_ATTENUATION(i);

				float4 clr = mainColor * _LightColor0 * diffuse * shadow;

				#if EmissMap_ON

				float4 offset1 = tex2D(_EmissMap, i.uv - _Time.yy * _EmissNoiseFactor / 10);
				float4 offset2 = tex2D(_EmissMap, i.uv + _Time.yy * _EmissNoiseFactor / 10);
				float2 uvOffset = float2(i.emissInfo.x - offset1.z * _EmissNoiseStrength / 100, i.emissInfo.y - offset2.z * _EmissNoiseStrength / 100);
				float emissR = tex2D(_EmissMap, uvOffset).r;
				float emissG = tex2D(_EmissMap, i.emissInfo.zw).g;
                float breath = 0;

                #if EmissBreathMap_ON
				fixed4 ebt = tex2D(_EmissBreathMap, _Time.xy * _EmissBreathSpeed);
                breath = ebt.x;
                #else
                breath = saturate(sin(_Time.y * _EmissBreathSpeed * 0.01) * 0.5 + 0.5);
				if (_EmissBreathSpeed == 0) breath = 1;
				#endif

				clr.rgb += _EmissColor * emissR * emissG * breath;               
                #endif

				#ifdef LIGHTMAP_ON
				float3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.texcoordLM.xy));
				clr.rgb = clr.rgb + mainColor.rgb * lm;
				#else
                clr.rgb += mainColor.rgb * i.vertexLight;
				#endif

				if (_DiffuseType != 0){
					clr.rgb += c.rgb * 0.5;
				}

                UNITY_APPLY_FOG(i.fogCoord,clr);
                return clr;
            }
            ENDCG
        }		
    }
    FallBack "Diffuse"
	CustomEditor "S_SceneDiffuse_GUI"
}