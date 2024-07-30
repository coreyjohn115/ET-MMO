Shader "Hidden/DeathGray"
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
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			uniform float _timeduration;

			half4 frag (v2f_img i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.uv);
				half gray = dot(col, half3(0.299, 0.587, 0.114));
				half4 graycol = float4(gray, gray, gray, 1);
				return lerp(col, graycol, _timeduration);
			}
			ENDCG
		}
	}
}
