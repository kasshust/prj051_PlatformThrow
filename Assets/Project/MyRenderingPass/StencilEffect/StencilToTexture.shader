Shader "Custom/StencilToTexture"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Stencil("Stencil Value", Range(0, 255)) = 2
    }
    
    SubShader
    {
        Cull Off 
        ZWrite Off 
        ZTest Always

        Stencil
        {
            Ref[_Stencil]
            Comp Equal
        }

        Pass
        {
            Name "StencilToTexture"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"

            sampler2D _MainTex;

            half4 frag(Varyings i) : SV_Target
            {

                half4 col = tex2D(_MainTex, i.uv);
                return float4(i.uv.x, i.uv.y,0.0,1.0);
            }

            ENDHLSL
        }
    }
}