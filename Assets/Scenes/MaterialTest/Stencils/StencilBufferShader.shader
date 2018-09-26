Shader "Custom/StencilBufferShader" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry+1"}
		Cull off
        ColorMask 0
        ZWrite off
		ZTest Always
        Stencil {
            Ref 1
            Comp always
            Pass replace
        }
		LOD 200

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0
		
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
