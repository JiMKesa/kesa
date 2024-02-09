Shader "LFOAdditive 2.0" {
	Properties {
		_Opacity ("Opacity", Range(0, 1)) = 1
		_FalloffStart ("FalloffStart", Range(0, 10)) = 5
		_FalloffStartPosition ("FalloffStartPosition", Range(-1, 1)) = 0
		_StartFadeIn ("StartFadeIn", Range(0, 10)) = 1
		_LenghtwisePosition ("LenghtwisePosition", Range(-1, 5)) = 0
		_LenghtwiseBrightness ("LenghtwiseBrightness", Float) = 1
		_SpeedRandomMultiplier ("SpeedRandomMultiplier", Float) = 0.1
		[NoScaleOffset] _NoiseTex ("NoiseTex", 2D) = "white" {}
		_NoiseContrast ("NoiseContrast", Range(1, 5)) = 1
		_Tilling ("Tilling", Vector) = (1,1,0,0)
		_Speed ("Speed", Vector) = (0,1,0,0)
		_Noise ("Noise", Range(0, 1)) = 0.5
		_Falloff ("Falloff", Range(0.5, 5)) = 4
		_Brightness ("Brightness", Range(0, 10)) = 1
		[HDR] _StartTint ("StartTint", Vector) = (1,1,1,1)
		[HDR] _EndTint ("EndTint", Vector) = (1,0,0,1)
		_TintFalloff ("TintFalloff", Range(0, 2)) = 1
		_Fresnel ("Fresnel", Range(0, 10)) = 1
		_FresnelInverted ("FresnelInverted", Range(0, 10)) = 0
		_NoiseFresnel ("NoiseFresnel", Range(0, 1)) = 0
		_ScaleRandomizer ("ScaleRandomizer", Range(0, 0.5)) = 0.05
		_LinearExpansion ("LinearExpansion", Range(-1, 1)) = 0
		_QuadraticExpansion ("QuadraticExpansion", Range(-1, 1)) = 0
		_Seed ("Seed", Range(-10, 10)) = 0
		[Toggle(_ENVIRONMENTREFLECTIONS_OFF)] _ENVIRONMENTREFLECTIONS_OFF ("_ENVIRONMENTREFLECTIONS_OFF", Float) = 1
		[Toggle(_SPECULARHIGHLIGHTS_OFF)] _SPECULARHIGHLIGHTS_OFF ("_SPECULARHIGHLIGHTS_OFF", Float) = 1
		[HideInInspector] _BUILTIN_QueueOffset ("Float", Float) = 0
		[HideInInspector] _BUILTIN_QueueControl ("Float", Float) = -1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "Hidden/Shader Graph/FallbackError"
}