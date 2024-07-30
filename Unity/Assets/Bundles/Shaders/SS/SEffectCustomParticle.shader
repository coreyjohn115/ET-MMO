Shader "Blz/Effect/CustomParticle"
{
    Properties
    {
		[HDR]_Color ("Color", Color) = (1, 1, 1,1)		
        _MainTex ("Texture", 2D) = "white" {}
		_MaskTex ("MaskTex", 2D) = "white" {}

		_Mode ("__mode", Float) = 0
		_SrcBlend ("__src", Float) = 1
		_DstBlend ("__dst", Float) = 0

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
			#pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_fog
			
			#pragma multi_compile _ MaskMap_On
			//**** Acting on UGUI clip.
			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			//**** To use ParticleSystem custom vertex streams.
			#pragma multi_compile_particles

            #include "UnityCG.cginc"
			#include "UnityUI.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float4 col : COLOR;
				//**** xy is uv. z is single particle age percent(0 - 1). w is MainTex tilling x.
                float4 tex0 : TEXCOORD0;
				//**** x is MainTex tilling y. y is MainTex uv x distance. z is MainTex uv y distance. w is MaskTex tilling x.
				float4 tex1 : TEXCOORD1;
				//**** x is MaskTex tilling y. y is MaskTex uv x distance. z is MaskTex uv y distance.
				float4 tex2 : TEXCOORD2;

				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 uvs : TEXCOORD0;
                float4 vertex : SV_POSITION;
				
				#if UNITY_UI_CLIP_RECT
				float2 objPos : TEXCOORD2;
				#endif

				float4 col : COLOR;
                //UNITY_FOG_COORDS(3)			

				UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex; float4 _MainTex_ST; 
			sampler2D _MaskTex; float4 _MaskTex_ST;
			float4 _Color;
			float4 _ClipRect;

			inline float2 B_TRANSFORM_TEX(float2 uv, float2 tilling, float4 st)
			{
				return uv * tilling + st.zw;
			}

            v2f vert (appdata v)
            {
                v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				#if UNITY_UI_CLIP_RECT
				o.objPos = v.vertex;
				#endif

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uvs.xy = B_TRANSFORM_TEX(v.tex0.xy, float2(v.tex0.w, v.tex1.x), _MainTex_ST);
				o.uvs.xy += float2(v.tex1.y, v.tex1.z);
				
				if (_Time.x <= 0)
				{
					o.uvs.xy = B_TRANSFORM_TEX(v.tex0.xy, float2(1, 1), _MainTex_ST);
				}

				#if MaskMap_On
                o.uvs.zw = B_TRANSFORM_TEX(v.tex0.xy, float2(v.tex1.w, v.tex2.x), _MaskTex_ST);
				o.uvs.zw += float2(v.tex2.y, v.tex2.z);
				#endif

				o.col = v.col;
				//UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uvs.xy) * i.col * _Color;
				
				#if MaskMap_On
				float4 mask = tex2D(_MaskTex, i.uvs.zw);
				col.rgb *= mask.r;
				#endif

                //UNITY_APPLY_FOG(i.fogCoord, col);

				#if UNITY_UI_CLIP_RECT
				col.a *= UnityGet2DClipping(i.objPos.xy, _ClipRect);
				#endif
                
				return col;
            }
            ENDCG
        }
    }
	CustomEditor "Blz_EffectCustomParticle_GUI"
}
