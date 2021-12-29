Shader "UI/HSV"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {} _Color("Tint", Color) = (1,1,1,1)
		// 色相
		_Hue("Hue", Range(0.0, 1.0)) = 0.0
		// 彩度
		_Saturation("Saturation", Range(0.0, 1.0)) = 0.5
		// 明度
		_Brightness("Brightness", Range(0.0, 1.0)) = 0.5
		// コントラスト
		_Contrast("Contrast", Range(0, 1.0)) = 0.5
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)]
		_UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" "CanUseSpriteAtlas" = "True" }
		Stencil { Ref[_Stencil] Comp[_StencilComp] Pass[_StencilOp] ReadMask[_StencilReadMask] WriteMask[_StencilWriteMask] }
		Cull Off Lighting Off ZWrite Off ZTest[unity_GUIZTestMode] Blend One OneMinusSrcAlpha ColorMask[_ColorMask]
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag 
			#pragma target 2.0
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP
			// 色相に応じて色を変える
			inline float3 tweekHue(float3 color, float hue) 
			{
				float3 k = float3(0.57735, 0.57735, 0.57735); float hueAngle = radians(hue); float cosHue = cos(hueAngle); float sinHue = sin(hueAngle); return color * cosHue + cross(k, color) * sinHue + k * dot(k, color) * (1 - cosHue);
			}
			
			// RGB カラーを HSV 変換する
			inline float4 convertToHSV(float4 rgbaColor, fixed4 hsvc)
			{
				float hue = 360 * hsvc.r;
				float saturation = hsvc.g * 2;
				float brightness = hsvc.b * 2 - 1;
				float contrast = hsvc.a * 2;
				float4 hsvcColor; hsvcColor.rgb = tweekHue(rgbaColor.rgb, hue);
				hsvcColor.rgb = (hsvcColor.rgb - 0.5f) * contrast + 0.5f;
				hsvcColor.rgb = hsvcColor.rgb + brightness;
				float3 intensity = dot(hsvcColor.rgb, float3(0.39, 0.59, 0.11));
				hsvcColor.rgb = lerp(intensity, hsvcColor.rgb, saturation);
				return hsvcColor; 
			}
			
			struct appdata_t 
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID 	
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				half4 mask : TEXCOORD2;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			sampler2D _MainTex; fixed4 _Color;

			// 色相/彩度/明度/コントラストをプロパティから受け取る
			fixed _Hue; fixed _Saturation;
			fixed _Brightness;
			fixed _Contrast;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float4 _MainTex_ST;
			float _MaskSoftnessX;
			float _MaskSoftnessY;
			
			// 頂点シェーダーはデフォルトシェーダーと同じ
			v2f vert(appdata_t v)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				float4 vPosition = UnityObjectToClipPos(v.vertex);
				OUT.worldPosition = v.vertex;
				OUT.vertex = vPosition;
				float2 pixelSize = vPosition.w;
				pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));
			
				float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
				float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
				OUT.texcoord = float4(v.texcoord.x, v.texcoord.y, maskUV.x, maskUV.y);
				OUT.mask = half4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_MaskSoftnessX, _MaskSoftnessY) + abs(pixelSize.xy)));
				OUT.color = v.color * _Color;
				return OUT;
			} 

			fixed4 frag(v2f IN) : SV_Target 
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
				
				#ifdef UNITY_UI_CLIP_RECT
				half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
				color.a *= m.x * m.y;
				#endif

				#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
				#endif
				
				// ここまではデフォルトシェーダーと同じ
				// ここから HSV 色空間の調整
				fixed4 hsvc = fixed4(_Hue, _Saturation, _Brightness, _Contrast);
				float4 hsvcColor = convertToHSV(color, hsvc);
				color.rgb = hsvcColor * color.a;
				return color;
			}
			ENDCG
		}
	}
}