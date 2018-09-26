Shader "Custom/LineRendererStandardShader" {
	Properties {
		_Albedo ("Albedo", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM

		#pragma surface surf Standard

		#pragma target 3.0

		struct Input {
			float4 color : COLOR;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Albedo;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = _Albedo;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Emission = IN.color.rgb;
			o.Alpha = IN.color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
