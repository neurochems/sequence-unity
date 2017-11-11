Shader "Unlit/dark_nucleus"
{
	Properties
	{
		f ("Frequency", float) = 20.0
		c1 ("Main Colour", Color) = (1,1,1,1)
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

			//float depth : DEPTH;

			float f;
			float4 c1 : COLOR;
			//float4 colour2;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 c = c1 / f;
  				float t1 = asin((_Time.z * c) / (i.uv.x*i.uv.y) % f);
  				float t2 = tan((_Time.z * c) / (i.uv.x/i.uv.y));
  				return (t1*(c1)*f*t2);
			}
			ENDCG
		}
	}
}
