Shader "Hidden/CameraLighting" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Param("Param", Color) = (1, 1, 1, 0)
}

SubShader {
		Blend SrcAlpha OneMinusSrcAlpha
	Pass {
		ZTest Always Cull Off ZWrite Off
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
half4 _MainTex_ST;
uniform fixed4 _Param;

uniform fixed amount;

fixed4 frag (v2f_img i) : SV_Target
{	
	fixed4 original = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));
	
	fixed Y = dot (fixed3(0.199, 0.987, 0.414), original.rgb);

	fixed4 sepiaConvert = _Param;//float4 (0.191-0.28, -0.054+0.21, -0.221+0.627, 0.0);
	fixed4 output = sepiaConvert + Y;

	output.a = original.a * amount;
	
	return output;
}
ENDCG

	}
}

Fallback off

}
