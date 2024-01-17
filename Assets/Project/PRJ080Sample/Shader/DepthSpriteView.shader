// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/2D/DepthSpriteView"
{
    Properties{
        [PerRendererData] _MainTex("Main texture", 2D) = "white" {}
        _DepthTexture("Depth texture", 2D) = "gray" {}
        _Gain("Gain", Range(-0.01, 0.01)) = 0.
        _Color("Tint", Color) = (1,1,1,1)
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
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                    o.projPos = ComputeScreenPos(o.pos);

                    o.uv = v.texcoord;
                    o.color = v.color;
                    return o;
                }

                sampler2D _DepthTexture;
                float _Gain;
                fixed4 _Color;

                fixed4 frag(v2f i) : SV_Target{

                    float val = tex2D(_DepthTexture, i.uv).r;
                    float3 difference = _WorldSpaceCameraPos - i.worldPos;

                    float  gain = _Gain;

                    float2 sampleXY = i.uv + float2(val* difference.x, val* difference.y) * gain;

                    float4 c = tex2D(_MainTex, sampleXY) * i.color;;

                    return c;
                }
                ENDCG
            }
        }
}