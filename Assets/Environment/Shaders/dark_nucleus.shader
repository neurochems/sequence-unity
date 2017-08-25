﻿Shader "Unlit/dark_nucleus"
{
	Properties
	{
		frequency ("Frequency", float) = 1.0
		colour1 ("Main Colour", Color) = (1,0,0,0)
		//colour2 ("Secondary Colour", Color) = (0,0,0,1)
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			float depth : DEPTH;

			float frequency;
			float4 colour1;
			//float4 colour2;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 c = sin(colour1);
  				float thing1 = asin((_Time.z * colour1) / (i.uv.x%i.uv.y) % frequency);
  				//float thing1 = cos((_Time.z * c) / (i.uv.x%i.uv.y) % frequency);
  				//float thing1 = cos((_Time.z * c) / (i.uv.x%i.uv.y) % frequency);
  				float thing2 = cos((_Time.z * c) / (i.uv.x%i.uv.y));
  				//return thing1*colour1*frequency;
  				return _Time.x * (thing1*(colour1)*frequency*thing2);
			}
			ENDCG
		}
	}
}