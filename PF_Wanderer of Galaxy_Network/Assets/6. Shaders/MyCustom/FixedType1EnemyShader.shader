Shader "MyCustom/FixedType1EnemyShader"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _NormalTex("Normal Map", 2D) = "bump" {}

        _RimColor ("RimColor", Color) = (1,1,1,1)
        _RimPower("Rim Power", Range(1, 10)) = 3


    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert 

        sampler2D _MainTex;
        sampler2D _NormalTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NormalTex;
            float3 viewDir;

        };
        fixed4 _RimColor;
        float _RimPower;



        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			float rim;// = dot(o.Normal, IN.viewDir);
            o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));
			rim = saturate(dot(o.Normal, IN.viewDir));
			o.Emission = pow(1 - rim, _RimPower) * _RimColor.rgb;
            o.Alpha = c.a;
        }
        ENDCG

    }
    FallBack "Diffuse"
}
