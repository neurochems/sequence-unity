Shader "Unlit/WorldSphere"
{
	Properties
	{
		frequency ("Frequency", float) = 20.0
		colour ("Main Colour", Color) = (1,1,1,1)
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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			float frequency;
			float4 colour;

			fixed4 frag (v2f i) : SV_Target
			{
				// make stripes
				//float stripe = floor(fmod((i.uv.x+i.uv.y) * (_Time.w + frequency), 2.0));
				float thing = asin((_Time.z * colour) / (i.uv.x%i.uv.y) % frequency);
				// apply stripe
				return thing*colour*frequency;
				//return stripe*colour;
			}
			ENDCG
		}
	}
}
