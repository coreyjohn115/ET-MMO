Shader "Unlit/CameraFogOfWar"
{
    Properties
    {
	    _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
		Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

		    struct appdata_t {
			    float4 vertex : POSITION;
			    float2 texcoord : TEXCOORD0;
		    };

		    struct v2f {
			    float4 vertex : SV_POSITION;
			    float2 texcoord : TEXCOORD0;
			    float4 screenPos : TEXCOORD1;
			    float3 viewVec : TEXCOORD2;
		    };

		    sampler2D _MainTex;float4 _MainTex_TexelSize;float4 _Color;
		    sampler2D _CameraDepthTexture;

		    uniform sampler2D _FOWMaskTex;
		    uniform float4 _FOWColor;
		    uniform sampler2D _FOWDepthTex;
		    uniform vector _FOWInfo;


		    float4 GetWorldPositionFromDepthValue( float2 uv, float linearDepth) 
            {
                float camPosZ = _ProjectionParams.y + (_ProjectionParams.z - _ProjectionParams.y) * linearDepth;

                float height = 2 * camPosZ / unity_CameraProjection._m11;
                float width = _ScreenParams.x / _ScreenParams.y * height;

                float camPosX = width * uv.x - width / 2;
                float camPosY = height * uv.y - height / 2;
                float4 camPos = float4(camPosX, camPosY, camPosZ, 1.0);
                return mul(unity_CameraToWorld, camPos);
            }

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord.xy;
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.texcoord.y = 1 - o.texcoord.y;
				#endif
				o.screenPos = ComputeScreenPos(o.vertex);
				float3 clipPos = float3((o.screenPos / o.screenPos.w * 2 - 1).xy, 1) * _ProjectionParams.z;
				o.viewVec = mul(unity_CameraInvProjection, clipPos.xyzz).xyz;
				return o;
			}
			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.texcoord);						
				float depth = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, i.screenPos)));
				float depth2 = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2Dproj(_FOWDepthTex, i.screenPos)));
				//depth = depth - saturate(depth - depth2);
				float3  viewPos = i.viewVec * depth;
				float3 worldPos = GetWorldPositionFromDepthValue(i.texcoord, depth).xyz;
				float2 maskUV = float2(worldPos.x / _FOWInfo.x, worldPos.z / _FOWInfo.y);
				float4 mask = tex2D(_FOWMaskTex, maskUV);
				//float4 keepCol = tex2D(_FOWKeepTex, i.texcoord);
				float visible = lerp(mask.g, mask.r, _FOWInfo.z);
				float depthVector = step(1, depth);
				float4 col2 = lerp(col, _FOWColor, (1 - visible) * 0.9);
				//col2 = lerp(col2, _FOWColor, (1 - mask.b) * 0.9);
				col = col2 * (1 - depthVector) + col * depthVector;
				return col;
			}
            ENDCG
        }
    }
}
