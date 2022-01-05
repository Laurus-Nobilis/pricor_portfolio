Shader "Custom/ThresholdTransparent"
{
	// The properties block of the Unity shader. In this example this block is empty
	// because the output color is predefined in the fragment shader code.
	Properties
	{
		_Threshold("Threshold", Range(0, 1)) = 0.5

		[PerRendererData] _MainTex("Main Tex", 2D) = "white"
		//[MainTexture] _BaseMap("Base Map", 2D) = "white"
	}

		// The SubShader block containing the Shader code.
		SubShader
	{
		// SubShader Tags define when and under which conditions a SubShader block or
		// a pass is executed.
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			// The HLSL code block. Unity SRP uses the HLSL language.
			HLSLPROGRAM
			// This line defines the name of the vertex shader.
			#pragma vertex vert
			// This line defines the name of the fragment shader.
			#pragma fragment frag

			// The Core.hlsl file contains definitions of frequently used HLSL
			// macros and functions, and also contains #include references to other
			// HLSL files (for example, Common.hlsl, SpaceTransforms.hlsl, etc.).
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			uniform float _Threshold;
	sampler2D _MainTex;

	// The structure definition defines which variables it contains.
	// This example uses the Attributes structure as an input structure in
	// the vertex shader.
	struct Attributes
	{
		// The positionOS variable contains the vertex positions in object
		// space.
		float4 positionOS   : POSITION;
		float2 uv			: TEXCOORD0;
	};

	struct Varyings
	{
		// The positions in this struct must have the SV_POSITION semantic.
		float4 positionHCS  : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	// The vertex shader definition with properties defined in the Varyings
	// structure. The type of the vert function must match the type (struct)
	// that it returns.
	Varyings vert(Attributes IN)
	{
		// Declaring the output object (OUT) with the Varyings struct.
		Varyings OUT;
		// The TransformObjectToHClip function transforms vertex positions
		// from object space to homogenous clip space.
		OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
		// Returning the output.
		OUT.uv = IN.uv;
		return OUT;
	}

	// The fragment shader definition.
	half4 frag(Varyings i) : SV_Target
	{
		half4 col = tex2D(_MainTex, i.uv);
		//float k = i.uv.y > _Threshold ? float3(1, 1, 1) : float3(0.3, 0.59, 0.11);
		//® Gray = Red * 0.3 + Green * 0.59 + Blue * 0.11
		//col.rgb = float (col.r * 0.3 + col.g * 0.59 + col.b * 0.11);
		if (i.uv.y > _Threshold)
		{
			col.rgb = dot(col.rgb, float3(0.3, 0.59, 0.11));  //ã‹L‚Ì®‚ª“àÏ‚ÌŒvZ‚¾‚Á‚½
		}

		return col;
		//half4 customColor = half4(col.rgb, alpha);

		//return customColor;
	}
	ENDHLSL
	}
	}
}