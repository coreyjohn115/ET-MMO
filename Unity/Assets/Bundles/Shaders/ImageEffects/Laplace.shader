// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hiddens/Laplace"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
//		Cull Off ZWrite Off ZTest Always

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
				fixed2 uv : TEXCOORD0;
			};

			struct v2f
			{
				fixed2 uv : TEXCOORD0;
				fixed4 vertex : SV_POSITION;
			};

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
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed2 uv1 = i.uv;
				uv1.x -= (_ScreenParams.z - 1);
				fixed2 uv2 = i.uv;
				uv2.y -= (_ScreenParams.w - 1);
//				fixed2 uv3 = i.uv;
//				uv3.x += (_ScreenParams.z - 1);
//				fixed2 uv4 = i.uv;
//				uv4.y += (_ScreenParams.w - 1);

				fixed4 col1 = tex2D(_MainTex, uv1);
				fixed4 col2 = tex2D(_MainTex, uv2);
//				fixed4 col3 = tex2D(_MainTex, uv3);
//				fixed4 col4 = tex2D(_MainTex, uv4);
//				fixed4 delta = 4 * col - col1 - col2 - col3 - col4;
				fixed4 delta = 2 * col - col1 - col2;

				fixed len = length(delta);
				col = lerp(delta + col, col, len);
				return col;
			}
			ENDCG
		}
	}
}
