Shader "Custom/SandBall" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Timer("Timer", Float) = 0.0
		_Frequency("Wobble Frequency", Float) = 0.1
		_Magnitude("Wobble Magnitude", Float) = 0.1
	}

	SubShader{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		struct Input 
		{
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		fixed4 _Color;
		uniform float _Magnitude;
		uniform float _Frequency;
		uniform float _Timer;

		void vert(inout appdata_full v) 
		{
			float PI = 3.1415;
			v.texcoord.xy += float2(_Timer*PI*0.1f, _Timer*PI*0.1f);
			v.vertex.x += _Magnitude * sin(_Frequency * 2 * (v.vertex.y + _Timer*PI));
			v.vertex.z += _Magnitude * cos(_Frequency*(v.vertex.y + _Timer*PI));
		}

		// Surface shader function
		void surf(Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
		}
		ENDCG
		}
	FallBack "Diffuse"
}
