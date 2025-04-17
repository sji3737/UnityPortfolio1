// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SpEp/TransparentRim"
{
	Properties
	{
		_Power("Power", Float) = 1
		_Scale("Scale", Float) = 1
		_Outer("Outer", Color) = (0,1,1,1)
		_Inner("Inner", Color) = (0.5566038,0,0,1)
		_TexturewithAlpha("Texture with Alpha", 2D) = "white" {}
		_Pattern("Pattern", 2D) = "white" {}
		_PatternColor("Pattern Color", Color) = (0,0,0,0)
		_AlphaScroll("Alpha Scroll", Vector) = (0,0,0,0)
		_PatternScroll("Pattern Scroll", Vector) = (0,0,0,0)
		_PatternTiling("Pattern Tiling", Vector) = (0,0,0,0)
		_Alphatiling("Alpha tiling", Vector) = (0,0,0,0)
		_ShieldStr("Shield Str", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform float _ShieldStr;
		uniform float4 _Inner;
		uniform float _Scale;
		uniform float _Power;
		uniform float4 _Outer;
		uniform float4 _PatternColor;
		uniform sampler2D _Pattern;
		uniform float2 _PatternScroll;
		uniform float2 _PatternTiling;
		uniform sampler2D _TexturewithAlpha;
		uniform float2 _AlphaScroll;
		uniform float2 _Alphatiling;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV1 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode1 = ( 0.0 + _Scale * pow( 1.0 - fresnelNdotV1, _Power ) );
			float2 uv_TexCoord22 = i.uv_texcoord * _PatternTiling;
			float2 panner20 = ( 1.0 * _Time.y * _PatternScroll + uv_TexCoord22);
			o.Emission = ( _ShieldStr * ( ( _Inner * ( 1.0 - fresnelNode1 ) ) + ( fresnelNode1 * _Outer ) + ( _PatternColor * tex2D( _Pattern, panner20 ) ) ) ).rgb;
			float2 uv_TexCoord24 = i.uv_texcoord * _Alphatiling;
			float2 panner25 = ( 1.0 * _Time.y * _AlphaScroll + uv_TexCoord24);
			o.Alpha = ( tex2D( _TexturewithAlpha, panner25 ).a * fresnelNode1 * _ShieldStr );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
7;153;1533;806;1486.73;799.6621;1.515403;True;False
Node;AmplifyShaderEditor.Vector2Node;27;-1904.98,-438.9968;Float;False;Property;_PatternTiling;Pattern Tiling;9;0;Create;True;0;0;False;0;0,0;8,5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-1675.02,-593.9158;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-1222,330;Float;False;Property;_Power;Power;0;0;Create;True;0;0;False;0;1;1.98;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;21;-1628.729,-388.757;Float;False;Property;_PatternScroll;Pattern Scroll;8;0;Create;True;0;0;False;0;0,0;0,0.4;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;2;-1214,171;Float;False;Property;_Scale;Scale;1;0;Create;True;0;0;False;0;1;1.91;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;26;-1488.244,617.2391;Float;False;Property;_Alphatiling;Alpha tiling;10;0;Create;True;0;0;False;0;0,0;2,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;20;-1361.943,-540.342;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FresnelNode;1;-1006.25,140.2516;Float;False;Standard;TangentNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;23;-1087.064,775.1807;Float;False;Property;_AlphaScroll;Alpha Scroll;7;0;Create;True;0;0;False;0;0,0;0,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-1133.355,570.0219;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;19;-1012.23,-853.1606;Float;False;Property;_PatternColor;Pattern Color;6;0;Create;True;0;0;False;0;0,0,0,0;0,0.3724051,1,0.627451;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-678.5,-414;Float;False;Property;_Inner;Inner;3;0;Create;True;0;0;False;0;0.5566038,0,0,1;0.8723254,0.5801887,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-1046.722,-608.2565;Float;True;Property;_Pattern;Pattern;5;0;Create;True;0;0;False;0;None;040925d7c501a5248948de8057d4a1fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;6;-668.3576,266.9909;Float;False;Property;_Outer;Outer;2;0;Create;True;0;0;False;0;0,1,1,1;0.5330188,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;11;-607.5563,-222.2974;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-586.2319,-687.5908;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;25;-820.2778,623.5957;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-401.5563,-380.2974;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-384.5563,-39.29742;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-148.6288,-637.514;Float;False;Property;_ShieldStr;Shield Str;11;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-389.6174,390.3379;Float;True;Property;_TexturewithAlpha;Texture with Alpha;4;0;Create;True;0;0;False;0;None;56d1b81a5eaba2d4db900c15308181e0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-178.556,-163.2974;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-86.07129,219.5933;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;5.017856,-330.4307;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;206.9625,-29.31969;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SpEp/TransparentRim;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;22;0;27;0
WireConnection;20;0;22;0
WireConnection;20;2;21;0
WireConnection;1;2;2;0
WireConnection;1;3;3;0
WireConnection;24;0;26;0
WireConnection;16;1;20;0
WireConnection;11;0;1;0
WireConnection;17;0;19;0
WireConnection;17;1;16;0
WireConnection;25;0;24;0
WireConnection;25;2;23;0
WireConnection;9;0;5;0
WireConnection;9;1;11;0
WireConnection;8;0;1;0
WireConnection;8;1;6;0
WireConnection;13;1;25;0
WireConnection;12;0;9;0
WireConnection;12;1;8;0
WireConnection;12;2;17;0
WireConnection;15;0;13;4
WireConnection;15;1;1;0
WireConnection;15;2;28;0
WireConnection;14;0;28;0
WireConnection;14;1;12;0
WireConnection;0;2;14;0
WireConnection;0;9;15;0
ASEEND*/
//CHKSM=878DF9926963B789977A49165E38D855D74341FE