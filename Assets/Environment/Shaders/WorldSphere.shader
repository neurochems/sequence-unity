Shader "Unlit/WorldSphere"
{
	Properties
	{
		f ("Frequency", float) = 20.0
		c ("Main Colour", Color) = (0,0,0,1)
		trans ("Transparency", Range(0.0, 1.0)) = 0.25
	}
	SubShader
	{

		Tags {"Queue"="Transparent" "RenderType"="Transparent"}
		LOD 100
		ZWrite Off
		//Cull Front
		Blend SrcAlpha OneMinusSrcAlpha

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

			float f;
			float4 c;
			float trans;

			fixed4 frag (v2f i) : SV_Target
			{
				// make stripes
				//float stripe = floor(fmod((i.uv.x+i.uv.y) * (_Time.w + frequency), 2.0));
				//float t = asin((_Time.z * c) / (i.uv.x%i.uv.y) % f);
				//fixed4 t2 = t*c*f;
				fixed4 col = c;
				col.a = trans;
				// apply stripe
				return col;
				//return stripe*colour;
			}
			ENDCG
		}
	}
}
