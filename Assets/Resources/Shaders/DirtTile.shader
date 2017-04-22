// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Resources/Shaders/DirtTile"
{
	Properties {
        // Diffuse texture
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _MaskTex ("Base (RGB)", 2D) = "white" {}
        _Cutoff ("Cutoff", Color) = (1, 1, 1, 0)
    }
    SubShader {
        Cull off ZWrite Off ZTest Always

        Tags 
         { 
             "RenderType" = "Opaque" 
             "Queue" = "Transparent+1" 
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
        	uniform float4 _Cutoff;

        	float4 frag (v2f i) : Color
        	{
        		fixed4 col = tex2D(_MainTex, i.uv_MainTex);
        		fixed4 col2 = tex2D(_MaskTex, i.uv_MainTex);
        		if(col2.a > 0 &&
        		   _Cutoff.r <= col2.r &&
        		   _Cutoff.g <= col2.g &&
        		   _Cutoff.b <= col2.b)
        		{
        			col.rgb = 0;
        			col.a = 1;
        		}
        		return col;
        	}
        	ENDCG
        }
    }
}
