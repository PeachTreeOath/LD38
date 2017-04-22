// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Curve/CurveWorld2"
{
	Properties {
        // Diffuse texture
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _ScreenWidth("_ScreenWidth", float) = 1080
        _CurveIntensity("_CurveIntensity", float) = .001
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
        	uniform float _CurveIntensity;

        	fixed4 frag (v2f i) : SV_Target
        	{
        		float pi = 3.14159;
        		float xPerc = i.vertex.x/(_ScreenWidth);
				float rescaleXperc = (xPerc - 0.5f) * 2;
        		float diff = pi * (rescaleXperc);
        		fixed4 col = tex2D(_MainTex, i.uv + float2(0, (diff*diff)/_CurveIntensity));
        		return col;
        	}
        	ENDCG
        }
    }
}
