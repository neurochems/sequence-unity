Shader "Unlit/Skybox"
{
	Properties
	{
		//colourmap ("Colour Map", 2D) = "black" {}

		frequency ("Frequency", float) = 20.0
		colour1 ("Main Colour", Color) = (1,1,1,1)
		colour2 ("Secondary Colour", Color) = (1,1,1,1)
		colour3 ("Tertiary Colour", Color) = (1,1,1,1)
	}
	SubShader
	{
		Pass
		{
			//CULL FRONT

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

			//uniform sampler2D colour_map;
			//float4 col : COLOR;
			float depth : DEPTH;

			float frequency;
			float4 colour1;
			float4 colour2;
			float4 colour3;

			fixed4 frag (v2f i) : SV_Target
			{
  				float4 c = sin(colour1 % colour2 % colour3);
  				//float thing1 = asin((_Time.z * colour1) / (i.uv.x%i.uv.y) % frequency);
  				float thing1 = asin((_Time.z * c) / (i.uv.x%i.uv.y) % frequency);
  				float thing2 = tan((_Time.z * c) / (i.uv.x%i.uv.y));
  				//return thing1*colour1*frequency;
  				return _Time.x * (thing1*(colour1+colour2-colour3)*frequency*thing2);
			}
			ENDCG
		}
	}
}
