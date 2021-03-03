﻿Shader "Hidden/PixelateEffect 1"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PixelWidth("Pixel Width", int) = 64
		_PixelHeight("Pixel Height", int) = 64
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
			
			int _PixelWidth;
			int _PixelHeight;
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.uv;
				uv.x *= _PixelWidth;
				uv.y *= _PixelHeight;
				uv.x = round(uv.x) / _PixelWidth;
				uv.y = round(uv.y) / _PixelHeight;
				
				fixed4 col = tex2D(_MainTex, uv);
				
				
				return col;
			}
			ENDCG
		}
	}
}
