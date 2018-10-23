Shader "Custom/SpiralMaterial" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_HeightFactor ("Height Slope", Range(0, 2)) = 1.0
		_HeightIntercept("Height Intercept", Range(-10, 10)) = 1.0
		_HeightColor("Height Color", Color) = (1, 1, 1, 1)
		_EmissionColor("Emission Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _HeightColor;
		fixed4 _EmissionColor;
		half _HeightFactor;
		half _HeightIntercept;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed heightBlend = saturate((_HeightIntercept - IN.worldPos.y) * _HeightFactor);
			o.Albedo = lerp(c.rgb, 0, heightBlend);
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			o.Emission = lerp(c.rgb * _EmissionColor.rgb, _HeightColor.rgb, heightBlend);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
