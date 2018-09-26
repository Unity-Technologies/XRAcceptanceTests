Shader "Custom/ArrowStandardShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_BaseColor("Base Color", Color) = (0.0, 0.0, 0.0, 0.0)
		_LerpFactor("Lerp Factor", Range(0.0,1.0)) = 0.0
		_LerpRate("Lerp Rate", Range(1, 3)) = 2
	}
	SubShader {
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 200
		Cull Off
		Blend SrcAlpha One

		CGPROGRAM
		#pragma surface surf Standard

		#pragma target 3.0

		struct Input {
			float4 color : COLOR;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _LerpFactor;
		fixed _LerpRate;
		fixed4 _TintColor;
		fixed4 _BaseColor;


		UNITY_INSTANCING_BUFFER_START(Props)

		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {

			float alpha = saturate(IN.color.r + (_LerpRate * _LerpFactor - 1.0));
			o.Albedo = 0.0;


			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			o.Emission = _BaseColor.rgb + _TintColor.rgb * alpha;
			o.Alpha = _BaseColor.a + _TintColor.a * alpha;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
