Shader "Hidden/HeatBlur"
{
     Properties   
    {  
        _MainTex ("Base (RGB)", 2D) = "white" {}  
        _NoiseTex ("Noise Texture (RG)", 2D) = "white" {}  
        _MaskTex ("Mask Texture", 2D) = "white" {}  
        _HeatTime  ("Heat Time", range (0,1.5)) = 1  
        _HeatForce  ("Heat Force", range (0,0.1)) = 0.1  
    }  

    SubShader   
    {  
        Pass  
        {  
            CGPROGRAM  
            #pragma vertex vert_img  
            #pragma fragment frag  
            #pragma fragmentoption ARB_precision_hint_fastest  
            #include "UnityCG.cginc"  

            fixed _HeatForce;  
            fixed _HeatTime;  

            uniform sampler2D _MainTex;  
            uniform sampler2D _NoiseTex;  
            uniform sampler2D _MaskTex;
            uniform fixed _Duration;  

            fixed4 frag(v2f_img i) : COLOR  
            {
            	fixed2 uv = i.uv - fixed2(0.5, 0.5);
            	uv *= _Duration;
            	uv += fixed2(0.5, 0.5);  
                fixed mask = tex2D(_MaskTex, uv).r;
                mask = saturate(mask);    

                // 扭曲效果  
                fixed4 offsetColor1 = tex2D(_NoiseTex, i.uv + _Time.xz * _HeatTime);  
                fixed4 offsetColor2 = tex2D(_NoiseTex, i.uv - _Time.yx * _HeatTime);  
                i.uv.x += ((offsetColor1.r + offsetColor2.r) - 1) * _HeatForce * mask;  
                i.uv.y += ((offsetColor1.g + offsetColor2.g) - 1) * _HeatForce * mask;  

                fixed4 renderTex = tex2D(_MainTex, i.uv);  

                return renderTex;  
            }  

            ENDCG  
        }  
    }   
    FallBack off  
}