// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/2D/DepthSprite"
{
    Properties{
        [PerRendererData] _MainTex("Main texture", 2D) = "white" {}
        _DepthTexture("Depth texture", 2D) = "gray" {}
        
        _XYGain("XYGain", Range(-1, 1)) = 0.
        _ZGain("ZGain", Range(0.0, 0.5)) = 0.
        _X("X", Range(-1.01, 1.01)) = 0.
        _Y("Y", Range(-1.01, 1.01)) = 0.

        _Color("Tint", Color) = (1,1,1,1)

        _OutLineSpread("OutLine Spread", Range(0.00, 0.05)) = 0.01
        _OutLineColor("Outline Color", Color) = (1, 1, 1, 1)

        _VignetColor("Vignet Color", Color) = (0, 0, 0, 1)
        _VignetColorStrength("_VignetColorStrength", Range(0, 10.0)) = 0.
    }

        SubShader{

            Tags { 
                "Queue" = "Transparent"
                "RenderType" = "Transparent"
            }

            Cull Off
            Lighting Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Pass {

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct v2f {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float3 worldPos : TEXCOORD1;
                    float4 projPos : TEXCOORD3;
                    fixed4 color : COLOR;
                };

                sampler2D _MainTex;

                v2f vert(appdata_full v) {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.texcoord;
                    o.color = v.color;
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    return o;
                }

                fixed4 SampleSpriteTexture(float2 uv)
                {
                    fixed4 color = tex2D(_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
                    if (_AlphaSplitEnabled)
                        color.a = tex2D(_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

                    return color;
                }

                sampler2D _DepthTexture;
                float _XYGain;
                float _X;
                float _Y;
                float _ZGain;
                fixed4 _Color;
                half _OutLineSpread;
                fixed4 _OutLineColor;
                fixed4 _VignetColor;
                float  _VignetColorStrength;

                fixed4 frag(v2f i) : SV_Target {
                    

                    float val = tex2D(_DepthTexture, i.uv).r;
                    float3 difference = _WorldSpaceCameraPos - i.worldPos;
                    float  zgain = _ZGain / 1000.0;
                    float2 sampleXY = i.uv + float2(_X * val, _Y * val) * _XYGain + float2(val * difference.x, val * difference.y) * zgain;
                    float4 c = tex2D(_MainTex, sampleXY) * i.color;

                    // アウトライン処理
                    fixed4 out_col = _OutLineColor;
                    _OutLineColor.a = 1;

                    float2 outlineUV = sampleXY;
                    half2 line_w = half2(_OutLineSpread, 0);
                    fixed4 line_col = SampleSpriteTexture(outlineUV + line_w.xy)
                        + SampleSpriteTexture(outlineUV - line_w.xy)
                        + SampleSpriteTexture(outlineUV + line_w.yx)
                        + SampleSpriteTexture(outlineUV - line_w.yx);
                    _OutLineColor *= (line_col.a) ;
                    _OutLineColor.rgb = out_col.rgb;
                    _OutLineColor = lerp(c, _OutLineColor, max(0, sign(_OutLineSpread)));

                    // 合成
                    fixed4 main_col = c;
                    main_col = lerp(main_col, _OutLineColor, (1 - main_col.a));

                    main_col *= i.color.a;

                    // 画面はしを暗く
                    float2 uv = i.uv - 0.5;
                    float vignet = length(uv);
                    main_col.rgb = lerp(main_col.rgb, _VignetColor.rgb, vignet*vignet*vignet* _VignetColorStrength);

                    return main_col;
                }
                ENDCG
            }
        }
}