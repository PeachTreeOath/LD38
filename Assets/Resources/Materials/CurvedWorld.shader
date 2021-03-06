﻿Shader "Custom/CurvedWorld" {
	Properties{
		// Diffuse texture
		_MainTex("Base (RGB)", 2D) = "white" {}
	// Degree of curvature
	_Curvature("Curvature", Float) = 0.001
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Surface shader function is called surf, and vertex preprocessor function is called vert
		// addshadow used to add shadow collector and caster passes following vertex modification
#pragma surface surf Unlit Lambert vertex:vert addshadow

		// Access the shaderlab properties
		uniform sampler2D _MainTex;
	uniform float _Curvature;

	// Basic input structure to the shader function
	// requires only a single set of UV texture mapping coordinates
	struct Input {
		float2 uv_MainTex;
	};

	// This is where the curvature is applied
	void vert(inout appdata_full v)
	{
		// Transform the vertex coordinates from model space into world space
		float4 vv = mul(unity_ObjectToWorld, v.vertex);

		// Now adjust the coordinates to be relative to the camera position
		vv.xyz -= _WorldSpaceCameraPos.xyz;

		// Modify the Y coordinate to warp the world space.
		vv = float4(0.0f, ((vv.x*-vv.x) / _Curvature) / (10), 0.0f, 0.0f);
		/*if (vv.x >= 0.0f)
		{
			vv = float4(0.0f, ((vv.x*-vv.x) / 5) / (vv.y + 10), 0.0f, 0.0f);
		}
		else
		{
			vv = float4(0.0f, (((vv.x*vv.x) / 5) / (vv.y + 10)), 0.0f, 0.0f);
		}*/


		// Now apply the offset back to the vertices in model space
		v.vertex += mul(unity_WorldToObject, vv);
	}

	half4 LightingUnlit(SurfaceOutput s, half3 lightDir, half atten)
	{
		return half4(s.Albedo, s.Alpha);
	}

	// This is just a default surface shader
	void surf(Input IN, inout SurfaceOutput o) {


		half4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
	}
		// FallBack "Diffuse"
}
