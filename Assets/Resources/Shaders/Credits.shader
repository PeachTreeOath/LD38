// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Resources/Shaders/Credits"
{
	Properties {
        // Diffuse texture
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _MaskTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Cull off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Tags 
         { 
             "RenderType" = "Transparent" 
             "Queue" = "Transparent" 
         }

        Pass
        {
        	CGPROGRAM
        	#pragma vertex vert
        	#pragma fragment frag

        	#include "UnityCG.cginc"

        	struct appdata
        	{
	        	float4 vertex : POSITION;
                float2 uv_MainTex : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
        	};

        	struct v2f
        	{
	        	float4 vertex : POSITION;
                float2 uv_MainTex : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
        	};

        	v2f vert (appdata v)
        	{
        		v2f o;
        		o.vertex = UnityObjectToClipPos(v.vertex);
        		o.uv_MainTex = v.uv_MainTex;
        		o.uv2 = v.uv2;
        		return o;
        	}

        	sampler2D _MainTex;
        	sampler2D _MaskTex;

        	float4 frag (v2f i) : Color
        	{
        		fixed4 col = tex2D(_MainTex, i.uv_MainTex);
        		if(col.r == 1 &&
        		   col.g == 1 &&
        		   col.b == 1 &&
        		   col.a == 1)
        		{
        			col = tex2D(_MaskTex, i.uv_MainTex);
        		}
        		return col;
        	}
        	ENDCG
        }
    }
}
