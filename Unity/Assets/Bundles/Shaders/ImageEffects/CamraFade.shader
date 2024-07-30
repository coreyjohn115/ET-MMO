// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/CamraFade"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Tex("CameraTex1", 2D) = "white" {}
	}
	SubShader
	{
		//Blend SrcAlpha OneMinusSrcAlpha
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"

			struct v2f
			{
				fixed2 uv : TEXCOORD0;
				fixed4 vertex : SV_POSITION;
			};

			uniform sampler2D _Tex;
			sampler2D _MainTex;
			fixed4 _Tex_ST;

				v2f vert(appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
//				fixed4 screenuv = ComputeGrabScreenPos(o.vertex);
//				o.uv = screenuv.xy;
//				o.uv = UnityStereoScreenSpaceUVAdjust(fixed2(
//					dot(v.texcoord.xy, fixed2(1, 0)),
//					dot(v.texcoord.xy, fixed2(0, 1))
//				), _Tex_ST);
//				#if UNITY_ANDROID
//				o.uv.y = 1 - o.uv.y;
//				#endif
				fixed4 pos = o.vertex * 0.5;
				o.uv = fixed2(pos.x + pos.w, pos.y + pos.w);
				//#if SHADER_API_MOBILE
				//o.uv.y = 1 - o.uv.y;
				//#endif 
				return o;
			}
			
			uniform fixed timeduration;

			fixed4 frag (v2f i) : SV_Target
			{	
				fixed4 col = tex2D(_Tex, i.uv);
				fixed4 col2 = tex2D(_MainTex, i.uv);
				col = lerp(col, col2, 1 - timeduration);
				return col;
			}
			ENDCG
		}
	}
}
