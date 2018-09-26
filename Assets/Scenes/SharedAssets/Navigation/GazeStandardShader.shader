Shader "Custom/GazeStandardShader" {
	Properties {
		_TintColor("Tint Color", Color) = (0, 0, 0, 0)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100
		ZTest Always
		Cull Off
		ZWrite Off
		Blend One One

		CGPROGRAM

		#pragma surface surf Standard 

		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _TintColor;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {

			o.Albedo = 0.0;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Emission = _TintColor.rgb;
			o.Alpha = _TintColor.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
