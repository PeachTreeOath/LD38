// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Curve/CurveWorld2"
{
	Properties {
        // Diffuse texture
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _ScreenWidth("_ScreenWidth", float) = 1920
        _ScreenHeight("_ScreenHeight", float) = 1080
        _CurveIntensity("_CurveIntensity", float) = .001
        _VignettePower ("VignettePower", Range(0.0,6.0)) = 1.5
    }
    SubShader {
        Cull off ZWrite Off ZTest Always

        Pass
        {
        	CGPROGRAM
        	#pragma vertex vert
        	#pragma fragment frag

        	#include "UnityCG.cginc"

        	struct appdata
        	{
	        	float4 vertex : POSITION;
	        	float2 uv : TEXCOORD0;
        	};

        	struct v2f
        	{
	        	float2 uv : TEXCOORD0;
	        	float4 vertex : SV_POSITION;
        	};

        	v2f vert (appdata v)
        	{
        		v2f o;
        		o.vertex = UnityObjectToClipPos(v.vertex);
        		o.uv = v.uv;
        		return o;
        	}

        	sampler2D _MainTex;
        	uniform float _ScreenWidth;
        	uniform float _ScreenHeight;
        	uniform float _CurveIntensity;
        	uniform float _VignettePower;


        	fixed4 frag (v2f i) : SV_Target
        	{
        		float pi = 3.14159;
        		float xPerc = i.vertex.x/(_ScreenWidth);
				float rescaleXperc = (xPerc - 0.5f) * 2;
        		float diff = pi * (rescaleXperc);
        		float yOffset = (diff*diff)/_CurveIntensity;
        		yOffset *= min(1, i.vertex.y/(_ScreenHeight*.3f));
        		fixed4 col = tex2D(_MainTex, i.uv + float2(0, yOffset));

        		float4 renderTex = tex2D(_MainTex, i.uv);
		        float2 dist = (i.uv - 0.5f) * 1.25f;
		        dist.x = 1 - dot(dist, dist)  * _VignettePower;
		        col *= dist.x; 

        		return col;
        	}
        	ENDCG
        }
    }
}
