Shader "MyCustom/FixedType2EnemyShader"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_NormalTex("Albedo (RGB)", 2D) = "white" {}
		_SpecPower("Specular Power", Float) = 1
		_SpecColor("Specular Color", Color) = (1,1,1,1)
		_TexColor("Texture Color", Color) = (1,1,1,1)
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }

			CGPROGRAM
			#pragma surface surf Test //noambient

			sampler2D _MainTex;
			sampler2D _NormalTex;

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_NormalTex;
			};
			float _SpecPower;
			float4 _TexColor;

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}

			float4 LightingTest(SurfaceOutput o, float3 lightDir, float3 viewDir, float atten)
			{
				float4 lighting;
				// diffuseColor 연산
				float ndotl = saturate(dot(lightDir, o.Normal)) * 0.5 + 0.5;
				float3 diffColor = ndotl * o.Albedo * _LightColor0.rgb * atten;

				//SpecularColor 연산
				float3 specColor;
				//블린퐁 연산
				float3 h = normalize(lightDir + viewDir); //하프벡터
				float spec = saturate(dot(o.Normal, h));
				//블린퐁 연산끝
				spec = pow(spec, _SpecPower);
				specColor = spec * _SpecColor;

				lighting.rgb = diffColor.rgb + specColor.rgb + _TexColor;
				lighting.a = o.Alpha;

				return lighting;
			}
			ENDCG
		}
			FallBack "Diffuse"

}
