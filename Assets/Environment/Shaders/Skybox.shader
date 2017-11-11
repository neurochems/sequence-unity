Shader "Unlit/Skybox"
{
	Properties
	{
		f ("Frequency", float) = 20.0
		_Fog ("Fog Value", Vector) = (0,0,0,0)
		c1 ("Main Colour", Color) = (1,1,1,1)
		c2 ("Secondary Colour", Color) = (1,1,1,1)
		c3 ("Tertiary Colour", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags {"Queue"="Background"}
		ZWrite Off
		CULL Back

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
				float3 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 uv : TEXCOORD0;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			float f;
			float4 c1 : COLOR;
			float4 c2 : COLOR;
			float4 c3 : COLOR;

			float4 _Fog;

			fixed4 frag (v2f i) : SV_Target
			{
  				float4 c = (c1 + c2 + c3) / f;
  				float t1 = asin((_Time.z * c) / (i.uv*i.uv) % f);
  				float t2 = tan((_Time.z * c) / (i.uv.x/i.uv.y));
  				float d = abs((i.uv.x/i.uv.y));
  				float col = lerp((t1*(c1+c2-c3)+(f*t2)), 0, smoothstep(_Fog.x, _Fog.y, d));
  				return col;
			}
			ENDCG
		}
	}
}
