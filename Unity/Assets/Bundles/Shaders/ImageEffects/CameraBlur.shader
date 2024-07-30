// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/CameraBlur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"

			struct appdata
			{
				fixed4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 vertex : SV_POSITION;
			};

			uniform fixed _BlurLength;
			uniform sampler2D _MaskTex;
			 
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				_BlurLength *= 0.01;
				//o.uv = v.uv;
				float2 uv0 = i.uv + float2(_BlurLength, _BlurLength);
				float2 uv1 = i.uv + float2(_BlurLength, -_BlurLength);
				float2 uv2 = i.uv + float2(-_BlurLength, _BlurLength);
				float2 uv3 = i.uv + float2(-_BlurLength, -_BlurLength);

				fixed4 col1 = tex2D(_MainTex, uv0);
				fixed4 col2 = tex2D(_MainTex, uv1);
				fixed4 col3 = tex2D(_MainTex, uv2);
				fixed4 col4 = tex2D(_MainTex, uv3);

				fixed4 col = col1 + col2 + col3 + col4;
				//fixed4 mask = tex2D(_MaskTex, i.uv);
				col *= 0.25;
				//col = lerp(col, mask, ceil(mask.r));

				return col;
			}
			ENDCG
		}
	}
}
