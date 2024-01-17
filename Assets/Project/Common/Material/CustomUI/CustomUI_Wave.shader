Shader "UI/CustomUI_Wave"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		_SinWave("SinWave", Range(0, 1)) = 0.2
		_SinWidth("SinWidth", Range(0, 1)) = 0.5
		_SinSpeed("SinSpeed", Range(0, 1)) = 0.2

		_SinWave2("SinWave2", Range(0, 1)) = 0.2
		_SinWidth2("SinWidth2", Range(0, 1)) = 0.5
		_SinSpeed2("SinSpeed2", Range(0, 1)) = 0.2

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}

			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask[_ColorMask]

			Pass
			{
				Name "Default"
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0

				#include "UnityCG.cginc"
				#include "UnityUI.cginc"

				#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
				#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
					float4 worldPosition : TEXCOORD1;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _MainTex;
				fixed4 _Color;
				fixed4 _TextureSampleAdd;
				float4 _ClipRect;
				float4 _MainTex_ST;

				float _SinWave;
				float _SinWidth;
				float _SinSpeed;

				float _SinWave2;
				float _SinWidth2;
				float _SinSpeed2;

				float _wave;
				float _speed;
				float _width;

				float _wave2;
				float _speed2;
				float _width2;

				v2f vert(appdata_t v)
				{
					v2f OUT;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
					OUT.worldPosition = v.vertex;
					OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

					OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

					OUT.color = v.color * _Color;
					return OUT;
				}

				float2 posColor(float2 inUV)
				{
					return inUV + float2(0, sin(inUV.x * _wave + _speed) * _width) + float2(0, sin(inUV.x * _wave2 + _speed2) * _width2);
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					float2 inUV = IN.texcoord;
					_wave = _SinWave * 50;
					_speed = _Time.y * _SinSpeed * 10.0;
					_width = _SinWidth * 0.5;

					_wave2 = _SinWave2 * 100;
					_speed2 = _Time.y * _SinSpeed2 * 20.0;
					_width2 = _SinWidth2 * 0.2;


					half4 color = (tex2D(_MainTex, posColor(inUV)) + _TextureSampleAdd) * IN.color;

					#ifdef UNITY_UI_CLIP_RECT
					color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
					#endif

					#ifdef UNITY_UI_ALPHACLIP
					clip(color.a - 0.001);
					#endif

					return color;
				}
			ENDCG
			}
		}
}