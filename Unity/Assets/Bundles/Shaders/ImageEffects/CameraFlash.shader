// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/CameraFlash"
{
	Properties
	{
		_Overlay("Overlay", 2D) = "white" {}
	}
	SubShader
	{	
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		ZTest Always Zwrite Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"

			struct v2f
			{
				half2 uv : TEXCOORD0;
				fixed4 vertex : SV_POSITION;
			};

			sampler2D _Overlay;
			fixed4 _Overlay_ST;

			v2f vert (appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = UnityStereoScreenSpaceUVAdjust(fixed2(
					dot(v.texcoord.xy, fixed2(1, 0)),
					dot(v.texcoord.xy, fixed2(0, 1))
					), _Overlay_ST);

				return o;
			}
		
			uniform fixed dir;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 m = (tex2D(_Overlay, i.uv));
				if (dir > 0)
					m.a *= dir;
				else
					m.a *= -dir;
				return m;
			}
			ENDCG
		}
	}
}
