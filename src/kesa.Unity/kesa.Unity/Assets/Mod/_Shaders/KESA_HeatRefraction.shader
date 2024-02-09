Shader "KESA/KESA_HeatRefraction" {
	Properties {
		[Header(Main)] _ScrollSpeedX ("Scroll Speed X", Float) = 0
		_Color ("Color", Vector) = (1,1,1,1)
		[NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}
		_ScrollSpeedY ("Scroll Speed Y", Float) = 0
		[Header(Refraction)] _RefractionAmount ("Refraction Amount", Range(0, 0.5)) = 0
		_BottomRefractionPosOffset ("Bottom Refraction Pos Offset", Range(0, 1)) = 0
		_BottomRefractionFalloffGradient ("Bottom Refraction Falloff Gradient", Range(0, 5)) = 0
		_TextureScaleX ("Texture Scale X", Float) = 1
		_TextureScaleY ("Texture Scale Y", Float) = 1
		[Normal] _RefractionTex ("RefractionTex", 2D) = "bump" {}
		[Header(Vertex Displacement)] _VertexDispScale ("Vertex Disp Scale", Range(0, 10)) = 0
		_VertexDispContrast ("Vertex Disp Contrast", Range(0.5, 5)) = 0
		_VertexDispPosOffset ("Vertex Disp Pos Offset", Range(0, 1)) = 0
		_VertexDispFalloffGradient ("Vertex Disp Falloff Gradient", Range(0, 3)) = 0
		[Header(Bending)] _AccelerationDir ("AccelerationDir", Vector) = (0,0,0,0)
		_AccelerationScaleFactor ("AccelerationScaleFactor", Float) = 0
		_BendRotationOffset ("BendRotationOffset", Float) = 0
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
}