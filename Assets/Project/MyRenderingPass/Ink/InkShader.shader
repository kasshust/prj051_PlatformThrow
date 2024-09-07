Shader "Universal Render Pipeline/2D/InkShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        // Legacy properties. They're here so that materials using this shader can gracefully fallback to the legacy sprite shader.
        [HideInInspector] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [HideInInspector] _AlphaTex ("External Alpha", 2D) = "white" {}
        [HideInInspector] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

            Pass
            {
                Tags { "LightMode" = "InkRender" }

                HLSLPROGRAM
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #if defined(DEBUG_DISPLAY)
                #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/InputData2D.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"
                #endif

                #pragma vertex UnlitVertex
                #pragma fragment UnlitFragment

                #pragma multi_compile _ DEBUG_DISPLAY

                struct Attributes
                {
                    float3 positionOS   : POSITION;
                    float4 color        : COLOR;
                    float2 uv           : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct Varyings
                {
                    float4  positionCS  : SV_POSITION;
                    half4   color       : COLOR;
                    float2  uv          : TEXCOORD0;
                    #if defined(DEBUG_DISPLAY)
                    float3  positionWS  : TEXCOORD2;
                    #endif
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                half4 _MainTex_ST;


                float4x4 newViewMatrix(float4x4 viewMatrix) {
                    float4x4 m;
                    /*
                    m._11_12_13_14 = viewMatrix._11_12_13_14;
                    m._21_22_23_24 = viewMatrix._21_22_23_24;
                    m._31_32_33_34 = viewMatrix._31_32_33_34;
                    m._41_42_43_44 = float4(0 , 0, 0.0, 1);
                    */
                    m._11_21_31_41 = viewMatrix._11_21_31_41;
                    m._12_22_32_42 = viewMatrix._12_22_32_42;
                    m._13_23_33_43 = viewMatrix._13_23_33_43;
                    m._14_24_34_44 = float4(0, 0, viewMatrix._14_24_34_44.z, 1);
                    

                    return m;
                }


                float4x4 newProjectionMatrix(float4x4 projectionMatrix) {
                    float4x4 m;

                    m._11_12_13_14 = float4(1.0, 0, 0.0, 1);
                    m._21_22_23_24 = float4(0, -1.0, 0.0, 1);
                    m._31_32_33_34 = float4(0, 0, 0.001, 1);
                    m._41_42_43_44 = float4(0, 0, 0.0, 1);

                    return m;
                }

                Varyings UnlitVertex(Attributes v)
                {
                    Varyings o = (Varyings)0;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                    // HLSL コードの頂点シェーダは、SpaceTransforms.hlsl ファイルの TransformObjectToHClip 関数を使用します。 この関数は、頂点位置をオブジェクト空間から均質空間に変換します：
                    // https://github.com/Unity-Technologies/Graphics/blob/master/Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl
                    // https://zenn.dev/r_ngtm/articles/urp-modify-camera-matrix
                    
                    // o.positionCS = TransformObjectToHClip(v.positionOS);
                    float3 WorldPos = TransformObjectToWorld(v.positionOS);

                    float4x4 newV = newViewMatrix(GetWorldToViewMatrix());
                    float4x4 newP = newProjectionMatrix(GetViewToHClipMatrix());

                    o.positionCS = mul(newP, mul(newV, float4(WorldPos,1.0f)));
                    // o.positionCS = mul(GetViewToHClipMatrix(), mul(GetWorldToViewMatrix(), float4(WorldPos, 1.0f)));


                    #if defined(DEBUG_DISPLAY)
                    o.positionWS = TransformObjectToWorld(v.positionOS);
                    #endif

                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = v.color;
                    return o;
                }

                half4 UnlitFragment(Varyings i) : SV_Target
                {
                    float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                    return float4(1.0, 1.0, 0.0, 1.0f);


                    #if defined(DEBUG_DISPLAY)
                    SurfaceData2D surfaceData;
                    InputData2D inputData;
                    half4 debugColor = 0;

                    InitializeSurfaceData(mainTex.rgb, mainTex.a, surfaceData);
                    InitializeInputData(i.uv, inputData);
                    SETUP_DEBUG_DATA_2D(inputData, i.positionWS);

                    if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
                    {
                        return debugColor;
                    }
                    #endif

                    return mainTex;
                }
                ENDHLSL
            }
    }

    Fallback "Sprites/Default"
}
