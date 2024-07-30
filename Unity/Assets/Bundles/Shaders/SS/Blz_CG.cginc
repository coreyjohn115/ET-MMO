//by fxc: English is not very well, so the tips will be not clear. 

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Lighting.cginc"

fixed shadow;half _Cutoff;
sampler2D _MainTex;float4 _MainTex_ST;fixed4 _Color;
sampler2D _MetallicGlossMap;float4 _MetallicGlossMap_ST;fixed _Smoothness;
sampler2D _BumpMap;float4 _BumpMap_ST;float _BumpScale;
sampler2D _EmissMap; float4 _EmissMap_ST;fixed4 _EmissColor;
sampler2D _EmissBreathMap;float4 _EmissBreathMap_ST;float _EmissMaskSpeedX; float _EmissMaskSpeedY;
float _EmissNoiseFactor; float _EmissNoiseStrength;float _EmissBreathSpeed;


////////////////////////////////////////////InOut-put/////////////////////////////////////////

// Base shaders should use this(Such as diffuse、 cutout).
struct VertexInputNormal
{
    float4 vertex   : POSITION;
    half3 normal    : NORMAL;
    float2 uv0      : TEXCOORD0;
    float2 uv1      : TEXCOORD1; 	//The lihgtMap will use uv1.(When we use dynamic lightmap, we should add uv2)
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

// PBR shader should use this.
struct VertexInputAdvanced
{
    float4 vertex   : POSITION;
    half3 normal    : NORMAL;
    float2 uv0      : TEXCOORD0;
    float2 uv1      : TEXCOORD1; 	//The lihgtMap will use uv1.(When we use dynamic lightmap, we should add uv2)
    half4 tangent  	: TANGENT;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct VertexOutputForwardBaseNormal
{
    UNITY_POSITION(pos);
    float2 uv                            	: TEXCOORD0;
    float3 viewDir                        	: TEXCOORD1;    
    float4 tangentToWorldAndWorldPos[3] 	: TEXCOORD2;   	// [3x3:tangentToWorld matrix | 1x3:worldPos]
    half4 lightmapUV             			: TEXCOORD5;   	// Lightmap UV
    UNITY_LIGHTING_COORDS(6,7)								// Only valid for ForwardBase 			
	float4 emissInfo : TEXCOORD9;
	UNITY_FOG_COORDS(10)

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

struct VertexOutputDeferredNormal
{
    UNITY_POSITION(pos);
    float2 uv                            	: TEXCOORD0;
    float3 viewDir                        	: TEXCOORD1;		
    half4 lightmapUV             			: TEXCOORD5; 	// Lightmap UVs
	float4 emissInfo 						: TEXCOORD7;

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

struct VertexOutputForwardBaseAdvanced
{
    UNITY_POSITION(pos);
    float2 uv                            	: TEXCOORD0;
    float3 viewDir                        	: TEXCOORD1;    
    float4 tangentToWorldAndWorldPos[3] 	: TEXCOORD2;   	// [3x3:tangentToWorld matrix | 1x3:worldPos]
    half4 lightmapUV             			: TEXCOORD5;  	// Lightmap UV
    UNITY_LIGHTING_COORDS(6,7)								// Only valid for ForwardBase 
	float4 emissInfo : TEXCOORD9;
	UNITY_FOG_COORDS(10)

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

struct VertexOutputDeferredAdvanced
{
    UNITY_POSITION(pos);
    float2 uv                            	: TEXCOORD0;
    float3 viewDir                        	: TEXCOORD1;		
    float4 tangentToWorldAndWorldPos[3] 	: TEXCOORD2;  	// [3x3:tangentToWorld matrix | 1x3: worldPos]
    half4 lightmapUV             			: TEXCOORD5; 	// Lightmap UVs
	float4 emissInfo 						: TEXCOORD7;

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

////////////////////////////////////////////InOut-put_End////////////////////////////////////


////////////////////////////////////////////StructData/////////////////////////////////////////

// Normal shaders have the common data. 
struct FragmentCommonData
{
    half3 diffColor, specColor;
    half oneMinusReflectivity, smoothness, occlusion;
    float3 normalWorld, viewDir, posWorld;
};

struct BlzLight
{
	half3 color;
	half3 dir;
};

struct BlzIndirect
{
	half3 diffuse;
	half3 specular;
};

struct BlzGIData
{
	BlzLight light;
	BlzIndirect indirect;
};

struct ToGBufferData
{
    half3   diffuseColor;
    half    occlusion;
    half3   specularColor;
    half    smoothness;
    float3  normalWorld;      
};

////////////////////////////////////////////StructData_End////////////////////////////////////

////////////////////////////////////InlineFunc//////////////////////////////////////////

//Get _MainTex alpha.
inline half Alpha(float2 uv)
{
	return tex2D(_MainTex, uv).a * _Color.a;
}

//Get _MainTex rgh.
inline half3 Albedo(float2 uv)
{
	return tex2D(_MainTex, uv).rgb * _Color.rgb;
}

//Get _MetallicMap info.
inline half3 MetallicInfo(float2 uv)
{
	half3 mg = 0;
	#if MetallicMap_ON
		mg = tex2D(_MetallicGlossMap, uv).rgb;
		mg.y *= _Smoothness;
	#else
		mg.z = 1;
	#endif
	return mg;
}

//Get worldNormal from bumpMap.
inline float3 PerPixelWorldNormal(float2 uv, float4 tangentToWorld[3])
{
    half3 tangent = tangentToWorld[0].xyz;
    half3 binormal = tangentToWorld[1].xyz;
    half3 normal = tangentToWorld[2].xyz;
	float3 normalTangent = UnpackNormal(tex2D(_BumpMap, uv));
	normalTangent.xy *= _BumpScale;
	normalTangent.z = sqrt(1.0 - saturate(dot(normalTangent.xy,normalTangent.xy)));
	// float3 normalWorld = normalize(fixed3(dot(tangent,normalTangent),
	// 					dot(binormal,normalTangent),dot(normal,normalTangent)));
	float3 normalWorld = normalize(tangent * normalTangent.x + binormal * normalTangent.y + normal * normalTangent.z);
    return normalWorld;
}

inline void ResetBlzGI(out BlzGIData gi)
{
	gi.light.color = 0;
	gi.light.dir = half3(0, 1, 0);
	gi.indirect.diffuse = 0;
	gi.indirect.specular = 0;
}

// Get ForwardBase 
inline BlzLight BlzLightForwardBase()
{
    BlzLight l;
    l.color = _LightColor0.rgb;
    l.dir = _WorldSpaceLightPos0.xyz;
    return l;
}

inline BlzLight BlzLightDeferred()
{
    BlzLight l;
    l.color = 0;
    l.dir = half3 (0,1,0);
    return l;
}

inline fixed3 ComputeDisneyDiffuseTerm(fixed nv,fixed nl,fixed lh, fixed roughness)
{
	fixed Fd90 = 0.5f + 2 * roughness * lh * lh;
	return (1 + (Fd90 - 1) * pow(1-nl,5)) * (1 + (Fd90 - 1) * pow(1-nv,5));
}

inline fixed ComputeSmithJointGGXVisibilityTerm(fixed nl, fixed nv, fixed roughness)
{
	fixed ag = roughness;
	fixed lambdaV = nl * (nv * (1 - ag) + ag);
	fixed lambdaL = nv * (nl * (1 - ag) + ag);
	
	return 0.5f/(lambdaV + lambdaL + 1e-5f);
}

inline fixed ComputeGGXTerm(fixed nh, fixed roughness)
{
	fixed a2 = roughness * roughness;
	fixed d = (a2 - 1.0f) * nh * nh + 1.0f;
	return a2 * UNITY_INV_PI / (d * d + 1e-7f);
}

inline fixed3 ComputeFresnelTerm(fixed3 F0,fixed cosA)
{
	return F0 + (1 - F0) * pow(1 - cosA, 5);
}

inline fixed3 ComputeFresnelLerp(fixed3 c0,fixed3 c1,fixed cosA)
{
	fixed t = pow(1 - cosA,5);
	return lerp(c0,c1,t);
}

//Compute lightmap`s uv. Input uv1 is static uv, input uv2 is dynamic uv.
inline fixed4 VertexGI(float2 uv1, float2 uv2, float3 posWorld, float3 normalWorld)
{
	fixed4 ambientOrLightmapUV = 0;

	#ifdef LIGHTMAP_ON
		ambientOrLightmapUV.xy = uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
	#elif UNITY_SHOULD_SAMPLE_SH
		ambientOrLightmapUV.rgb += ShadeSH9(fixed4(normalWorld,1));
	#endif

	return ambientOrLightmapUV;
}

//Get global illumination(Baked color、occlusion)
inline BlzGIData FragmentGI(FragmentCommonData s, half occlusion, half4 lightmapUV, half atten, BlzLight light)
{
	BlzGIData gi;
	ResetBlzGI(gi);
	gi.light = light;
	gi.light.color *= atten;

	#ifdef LIGHTMAP_ON
		half3 bakedColor = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, lightmapUV));
		gi.indirect.diffuse = bakedColor;
	#else
		gi.indirect.diffuse = lightmapUV;		
	#endif
	gi.indirect.diffuse *= occlusion;
	return gi;
}

// Get the normal info.
inline FragmentCommonData ComputeCommonData(float2 uv, float3 viewDir, float4 tangentToWorld[3], float3 posWorld)
{
	//todo: Whether to support cutout?
    half alpha = Alpha(uv);
    #if defined(AlphaTest_ON)
        clip (alpha - _Cutoff);
    #endif

    half3 metallicInfo = MetallicInfo(uv);
    half metallic = metallicInfo.x;	
    half smoothness = metallicInfo.y; 
    half occlusion = metallicInfo.z;

    half oneMinusReflectivity;
    half3 specColor;
    //According to the metallicmap`s r channel, we can get 
    half3 diffColor = DiffuseAndSpecularFromMetallic (Albedo(uv), metallic, specColor, oneMinusReflectivity);

    FragmentCommonData o = (FragmentCommonData)0;
    o.diffColor = diffColor;
    o.specColor = specColor;
    o.oneMinusReflectivity = oneMinusReflectivity;
    o.smoothness = smoothness;
    o.occlusion = occlusion;
	o.normalWorld = PerPixelWorldNormal(uv, tangentToWorld);
    o.viewDir = normalize(viewDir);
    o.posWorld = posWorld;
    return o;
}

inline half4 UnityBRDR(half3 diffColor, half3 specColor, half oneMinusReflectivity, half smoothness,
    float3 normalWorld, float3 viewDir, BlzLight light, UnityIndirect gi)
{	
	float perceptualRoughness = 1 - smoothness;
	fixed3 halfDir = normalize(light.dir + viewDir);
	fixed nv = abs(dot(normalWorld, viewDir));
    fixed nl = saturate(dot(normalWorld, light.dir));
    fixed nh = saturate(dot(normalWorld, halfDir));
    fixed lv = saturate(dot(light.dir, viewDir));
    fixed lh = saturate(dot(light.dir, halfDir));

	half diffuseTerm = ComputeDisneyDiffuseTerm(nv,nl,lh,perceptualRoughness) * nl;

	float roughness = max(perceptualRoughness * perceptualRoughness, 0.002);
    float V = SmithJointGGXVisibilityTerm (nl, nv, roughness);
    float D = GGXTerm (nh, roughness);
	fixed3 specularTerm = V * D * UNITY_PI;
	specularTerm = sqrt(max(1e-4h, specularTerm));
	specularTerm = max(0, specularTerm * nl);	
	specularTerm *= any(specColor) ? 1.0 : 0.0;

	half surfaceReduction;
	surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;

    half grazingTerm = saturate(smoothness + (1-oneMinusReflectivity));
    half3 color =   diffColor * (gi.diffuse + light.color * diffuseTerm)
                    + specularTerm * light.color * ComputeFresnelTerm (specColor, lh)
                    + surfaceReduction * gi.specular * ComputeFresnelLerp (specColor, grazingTerm, nv);

    return half4(color, 1);
}

inline half4 Emission(float2 uv, float4 emissInfo)
{
	#if EmissMap_ON
		float4 offset1 = tex2D(_EmissMap, uv - _Time.yy * _EmissNoiseFactor / 10);
		float4 offset2 = tex2D(_EmissMap, uv + _Time.yy * _EmissNoiseFactor / 10);
		float2 uvOffset = float2(emissInfo.x - offset1.z * _EmissNoiseStrength / 100, emissInfo.y - offset2.z * _EmissNoiseStrength / 100);
		float emissR = tex2D(_EmissMap, uvOffset).r;
		float emissG = tex2D(_EmissMap, emissInfo.zw).g;
	    float breath = 0;

	    #if EmissBreathMap_ON
			fixed4 ebt = tex2D(_EmissBreathMap, _Time.xy * _EmissBreathSpeed);
		    breath = ebt.x;
	    #else
		    breath = saturate(sin(_Time.y * _EmissBreathSpeed * 0.01) * 0.5 + 0.5);
			if (_EmissBreathSpeed == 0) breath = 1;
		#endif

		return _EmissColor * emissR * emissG * breath;               
    #endif

    return half4(0, 0, 0, 0);
}

inline void ComputeToGbuffer(ToGBufferData data, out half4 outGBuffer0, out half4 outGBuffer1, out half4 outGBuffer2)
{
    // RT0: diffuse color (rgb), occlusion (a) - sRGB rendertarget
    outGBuffer0 = half4(data.diffuseColor, data.occlusion); 

    // RT1: spec color (rgb), smoothness (a) - sRGB rendertarget
    outGBuffer1 = half4(data.specularColor, data.smoothness);

    // RT2: normal (rgb), --unused, very low precision-- (a)
    outGBuffer2 = half4(data.normalWorld * 0.5f + 0.5f, 1.0f);
}

////////////////////////////////////InlineFunc_End/////////////////////////////////////

/////////////////////////////////////CGPROGRAM///////////////////////////////////////////
VertexOutputForwardBaseAdvanced vertForwardBaseAdvanced(VertexInputAdvanced v)
{
    UNITY_SETUP_INSTANCE_ID(v);
    VertexOutputForwardBaseAdvanced o;
    UNITY_INITIALIZE_OUTPUT(VertexOutputForwardBaseAdvanced, o);
    UNITY_TRANSFER_INSTANCE_ID(v, o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv0, _MainTex);

	o.emissInfo.xy = TRANSFORM_TEX(v.uv0, _EmissMap).xy;
	o.emissInfo.zw = o.emissInfo.xy + _Time.z * float2(_EmissMaskSpeedX / 100, _EmissMaskSpeedY / 100);

    float3 posWorld = mul(unity_ObjectToWorld, v.vertex);
    float3 normalWorld = UnityObjectToWorldNormal(v.normal);
    float3 tangentWorld = UnityObjectToWorldDir(v.tangent);
    float3x3 tangentToWorldMatrix = CreateTangentToWorldPerVertex(normalWorld, tangentWorld.xyz, v.tangent.w);
    o.tangentToWorldAndWorldPos[0] = float4(tangentToWorldMatrix[0], posWorld.x);
    o.tangentToWorldAndWorldPos[1] = float4(tangentToWorldMatrix[1], posWorld.y);
    o.tangentToWorldAndWorldPos[2] = float4(tangentToWorldMatrix[2], posWorld.z);
    o.lightmapUV = VertexGI(v.uv1, v.uv1, posWorld, normalWorld);	// only need static lightmap uv.
    o.viewDir = UnityWorldSpaceViewDir(posWorld);

	TRANSFER_SHADOW(o);
	UNITY_TRANSFER_FOG(o,o.pos);
	return o;
}

fixed4 fragForwardBaseAdvanced(VertexOutputForwardBaseAdvanced i) : SV_Target
{
	float3 posWorld = float3(i.tangentToWorldAndWorldPos[0].w, i.tangentToWorldAndWorldPos[1].w, i.tangentToWorldAndWorldPos[2].w);
	
	FragmentCommonData s = ComputeCommonData(i.uv, i.viewDir, i.tangentToWorldAndWorldPos, posWorld);

    UNITY_SETUP_INSTANCE_ID(i);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
	UNITY_LIGHT_ATTENUATION(atten, i, posWorld); //Compute shadow atten.
	
	BlzLight blzL = BlzLightForwardBase();
	BlzGIData gi = FragmentGI(s, s.occlusion, i.lightmapUV, atten, blzL);
	half4 color = UnityBRDR(s.diffColor, s.specColor, s.oneMinusReflectivity, s.smoothness, s.normalWorld, s.viewDir, gi.light, gi.indirect);
	color += Emission(i.uv, i.emissInfo);

	UNITY_APPLY_FOG(i.fogCoord, color.rgb);
	return color;
}

VertexOutputDeferredAdvanced vertDeferredAdvanced(VertexInputAdvanced v)
{
	UNITY_SETUP_INSTANCE_ID(v);
    VertexOutputDeferredAdvanced o;
    UNITY_INITIALIZE_OUTPUT(VertexOutputDeferredAdvanced, o);
    UNITY_TRANSFER_INSTANCE_ID(v, o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv0, _MainTex);

	o.emissInfo.xy = TRANSFORM_TEX(v.uv0, _EmissMap).xy;
	o.emissInfo.zw = o.emissInfo.xy + _Time.z * float2(_EmissMaskSpeedX / 100, _EmissMaskSpeedY / 100);

    float3 posWorld = mul(unity_ObjectToWorld, v.vertex);
    float3 normalWorld = UnityObjectToWorldNormal(v.normal);
    float3 tangentWorld = UnityObjectToWorldDir(v.tangent);
    float3x3 tangentToWorldMatrix = CreateTangentToWorldPerVertex(normalWorld, tangentWorld.xyz, v.tangent.w);
    o.tangentToWorldAndWorldPos[0] = float4(tangentToWorldMatrix[0], posWorld.x);
    o.tangentToWorldAndWorldPos[1] = float4(tangentToWorldMatrix[1], posWorld.y);
    o.tangentToWorldAndWorldPos[2] = float4(tangentToWorldMatrix[2], posWorld.z);
    o.lightmapUV = VertexGI(v.uv1, v.uv1, posWorld, normalWorld);	// only need static lightmap uv.
    o.viewDir = UnityWorldSpaceViewDir(posWorld);

	return o;
}

void fragDeferredAdvanced(VertexOutputDeferredAdvanced i, out half4 outGBuffer0 : SV_Target0, out half4 outGBuffer1 : SV_Target1,
					out half4 outGBuffer2 : SV_Target2, out half4 outEmission : SV_Target3)
{
	outGBuffer0 = 1;outGBuffer1 = 1;outGBuffer2 = 0;outEmission = 0;
	UNITY_SETUP_INSTANCE_ID(i);

	float3 posWorld = float3(i.tangentToWorldAndWorldPos[0].w, i.tangentToWorldAndWorldPos[1].w, i.tangentToWorldAndWorldPos[2].w);
	
	FragmentCommonData s = ComputeCommonData(i.uv, i.viewDir, i.tangentToWorldAndWorldPos, posWorld);

    UNITY_SETUP_INSTANCE_ID(i);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
	
	BlzLight blzL = BlzLightDeferred();
	BlzGIData gi = FragmentGI(s, s.occlusion, i.lightmapUV, 1, blzL);
	half4 color = UnityBRDR(s.diffColor, s.specColor, s.oneMinusReflectivity, s.smoothness, s.normalWorld, s.viewDir, gi.light, gi.indirect);
	color += Emission(i.uv, i.emissInfo);

	ToGBufferData data;
	data.diffuseColor   = s.diffColor;
	data.occlusion      = s.occlusion;
	data.specularColor  = s.specColor;
	data.smoothness     = s.smoothness;
	data.normalWorld    = s.normalWorld;

	ComputeToGbuffer(data, outGBuffer0, outGBuffer1, outGBuffer2);

	outEmission = fixed4(color.rgb, 1);
} 

////////////////////////////////////CGPROGRAM_End////////////////////////////////////////