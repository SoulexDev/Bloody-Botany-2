// Made with Amplify Shader Editor v1.9.9.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AS_Particles"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0
		[Toggle( _POLAR_ON )] _Polar( "Polar?", Float ) = 0
		_RadialScale( "Radial Scale", Float ) = 0
		_LengthScale( "Length Scale", Float ) = 0
		_Center( "Center", Vector ) = ( 0, 0, 0, 0 )
		_Main_Tex( "Main_Tex", 2D ) = "white" {}
		_Main_Steps( "Main_Steps", Float ) = 180
		[Toggle( _INVERT_ON )] _Invert( "Invert?", Float ) = 0
		[Toggle( _USE_VERT_COLOR_ON )] _Use_Vert_Color( "Use_Vert_Color?", Float ) = 0
		_Vertex_Fade( "Vertex_Fade", Float ) = 1
		[Toggle( _CONTROL_OFFSET_ON )] _Control_Offset( "Control_Offset?", Float ) = 0
		_TextureRotation( "Texture Rotation", Float ) = 0
		_Tiling_X( "Tiling_X", Float ) = 1
		_Tiling_Y( "Tiling_Y", Float ) = 1
		_Offset_X( "Offset_X", Float ) = 0
		_Offset_Y( "Offset_Y", Float ) = 0
		_MainSpeedX( "Main Speed X", Float ) = 0
		_MainSpeedY( "Main Speed Y", Float ) = 0
		_Power( "Power", Float ) = 1
		_Emissive( "Emissive", Float ) = 1
		[Toggle( _CONTROL_EMISSIVE_ON )] _Control_Emissive( "Control_Emissive?", Float ) = 0
		_DepthFade( "Depth Fade", Range( 0, 10 ) ) = 1
		[Header(Distortion Noise)] _Noise_Tex( "Noise_Tex", 2D ) = "white" {}
		[Toggle( _POLARNOISE_ON )] _PolarNoise( "Polar Noise?", Float ) = 0
		_Noise_Steps( "Noise_Steps", Float ) = 180
		_Noise_Power( "Noise_Power", Float ) = 1
		_NoiseRotation( "Noise Rotation", Float ) = 0
		_Noise_Intensity( "Noise_Intensity", Float ) = 0
		_Noise_Tiling_X( "Noise_Tiling_X", Float ) = 1
		_Noise_Tiling_Y( "Noise_Tiling_Y", Float ) = 1
		_NoiseSpeedX( "Noise Speed X", Float ) = 0
		_NoiseSpeedY( "Noise Speed Y", Float ) = 0
		_Noise_Offset_X( "Noise_Offset_X", Float ) = 0
		_Noise_Offset_Y( "Noise_Offset_Y", Float ) = 0
		[Header(Gradient)] _Gradient_Tex( "Gradient_Tex", 2D ) = "white" {}
		_GradientRotation( "Gradient Rotation", Float ) = 0
		_Gradient_Intensity( "Gradient_Intensity", Float ) = 0
		_Gradient_Tiling_X( "Gradient_Tiling_X", Float ) = 1
		_Gradient_Tiling_Y( "Gradient_Tiling_Y", Float ) = 1
		_Gradient_Offset_X( "Gradient_Offset_X", Float ) = 0
		_Gradient_Offset_Y( "Gradient_Offset_Y", Float ) = 0
		[Header(Noise)] _Noise02_tex( "Noise02_tex", 2D ) = "white" {}
		[Toggle( _USE_NOISE_ON )] _Use_Noise( "Use_Noise?", Float ) = 0
		_Noise02_Intensity( "Noise02_Intensity", Float ) = 0
		_Noise02_Tiling_X( "Noise02_Tiling_X", Float ) = 1
		_Noise02_Tiling_Y( "Noise02_Tiling_Y", Float ) = 1
		_Noise02_Speed_X( "Noise02_Speed_X", Float ) = 0
		_Noise02_Speed_Y( "Noise02_Speed_Y", Float ) = 0
		[Header(WPO)] _WPO_tex( "WPO_tex", 2D ) = "white" {}
		_WPO_X_Tiling( "WPO_X_Tiling", Float ) = 0
		_WPO_Y_Tiling( "WPO_Y_Tiling", Float ) = 0
		_WPO_Direction( "WPO_Direction", Vector ) = ( 1, 1, 1, 0 )
		_WPO_power( "WPO_power", Float ) = 0
		[Enum(World,0,Vertex,1)] _World_or_Vertex( "World_or_Vertex", Float ) = 0
		_WPO_power_02( "WPO_power_02", Float ) = 0
		_WPO_U_Speed( "WPO_U_Speed", Float ) = 0
		[Enum(OFF,0,ON,1)] _WWPO_SC( "WWPO_S/C", Float ) = 0
		_WPO_V_Speed( "WPO_V_Speed", Float ) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord3( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.5
		#pragma shader_feature_local _CONTROL_EMISSIVE_ON
		#pragma shader_feature_local _USE_NOISE_ON
		#pragma shader_feature_local _INVERT_ON
		#pragma shader_feature_local _POLAR_ON
		#pragma shader_feature_local _CONTROL_OFFSET_ON
		#pragma shader_feature_local _POLARNOISE_ON
		#pragma shader_feature_local _USE_VERT_COLOR_ON
		#define ASE_VERSION 19905
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 uv3_texcoord3;
			float4 screenPos;
		};

		uniform sampler2D _WPO_tex;
		uniform float _WPO_U_Speed;
		uniform float _WPO_V_Speed;
		uniform float _WPO_X_Tiling;
		uniform float _WPO_Y_Tiling;
		uniform float _WPO_power;
		uniform float _WPO_power_02;
		uniform float _WWPO_SC;
		uniform float _World_or_Vertex;
		uniform float4 _WPO_Direction;
		uniform sampler2D _Gradient_Tex;
		uniform float _Gradient_Tiling_X;
		uniform float _Gradient_Tiling_Y;
		uniform float _Gradient_Offset_X;
		uniform float _Gradient_Offset_Y;
		uniform float _GradientRotation;
		uniform float _Gradient_Intensity;
		uniform sampler2D _Main_Tex;
		uniform float _MainSpeedX;
		uniform float _MainSpeedY;
		uniform float _Tiling_X;
		uniform float _Tiling_Y;
		uniform float _Offset_X;
		uniform float _Offset_Y;
		uniform float _RadialScale;
		uniform float2 _Center;
		uniform float _LengthScale;
		uniform float _TextureRotation;
		uniform sampler2D _Noise_Tex;
		uniform float _NoiseSpeedX;
		uniform float _NoiseSpeedY;
		uniform float _Noise_Tiling_X;
		uniform float _Noise_Tiling_Y;
		uniform float _Noise_Offset_X;
		uniform float _Noise_Offset_Y;
		uniform float _NoiseRotation;
		uniform float _Noise_Steps;
		uniform float _Noise_Intensity;
		uniform float _Noise_Power;
		uniform float _Main_Steps;
		uniform float _Power;
		uniform sampler2D _Noise02_tex;
		uniform float _Noise02_Speed_X;
		uniform float _Noise02_Speed_Y;
		uniform float _Noise02_Tiling_X;
		uniform float _Noise02_Tiling_Y;
		uniform float _Noise02_Intensity;
		uniform float _Emissive;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthFade;
		uniform float _Vertex_Fade;
		uniform float _Cutoff = 0;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 appendResult116 = (float2(_WPO_U_Speed , _WPO_V_Speed));
			float2 appendResult133 = (float2(_WPO_X_Tiling , _WPO_Y_Tiling));
			float2 uv_TexCoord115 = v.texcoord.xy * appendResult133;
			float2 panner122 = ( 1.0 * _Time.y * appendResult116 + uv_TexCoord115);
			float lerpResult126 = lerp( _WPO_power , _WPO_power_02 , _WWPO_SC);
			float3 ase_normalWS = UnityObjectToWorldNormal( v.normal );
			float3 ase_normalOS = v.normal.xyz;
			float3 lerpResult124 = lerp( ase_normalWS , ase_normalOS , _World_or_Vertex);
			float4 WPO128 = ( tex2Dlod( _WPO_tex, float4( panner122, 0, 0.0) ).r * lerpResult126 * float4( lerpResult124 , 0.0 ) * _WPO_Direction );
			v.vertex.xyz += WPO128.xyz;
			v.vertex.w = 1;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult68 = (float2(_Gradient_Tiling_X , _Gradient_Tiling_Y));
			float2 appendResult67 = (float2(_Gradient_Offset_X , _Gradient_Offset_Y));
			float2 uv_TexCoord61 = i.uv_texcoord * appendResult68 + appendResult67;
			float cos60 = cos( degrees( _GradientRotation ) );
			float sin60 = sin( degrees( _GradientRotation ) );
			float2 rotator60 = mul( uv_TexCoord61 - float2( 0,0 ) , float2x2( cos60 , -sin60 , sin60 , cos60 )) + float2( 0,0 );
			float3 Gradient105 = ( tex2D( _Gradient_Tex, rotator60 ).rgb * _Gradient_Intensity );
			float2 appendResult13 = (float2(_MainSpeedX , _MainSpeedY));
			float2 appendResult6 = (float2(_Tiling_X , _Tiling_Y));
			float2 appendResult47 = (float2(_Offset_X , _Offset_Y));
			float2 appendResult100 = (float2(i.uv3_texcoord3.x , i.uv3_texcoord3.y));
			#ifdef _CONTROL_OFFSET_ON
				float2 staticSwitch99 = appendResult100;
			#else
				float2 staticSwitch99 = appendResult47;
			#endif
			float2 uv_TexCoord92 = i.uv_texcoord * appendResult6 + staticSwitch99;
			float2 temp_output_34_0_g2 = ( i.uv_texcoord - _Center );
			float2 break39_g2 = temp_output_34_0_g2;
			float2 appendResult50_g2 = (float2(( _RadialScale * ( length( temp_output_34_0_g2 ) * 2.0 ) ) , ( ( atan2( break39_g2.x , break39_g2.y ) * ( 1.0 / 6.28318548202515 ) ) * _LengthScale )));
			float2 temp_output_89_0 = ( appendResult50_g2 * appendResult6 );
			float2 uv_TexCoord3 = i.uv_texcoord * temp_output_89_0 + ( temp_output_89_0 + appendResult47 );
			#ifdef _POLAR_ON
				float2 staticSwitch90 = uv_TexCoord3;
			#else
				float2 staticSwitch90 = uv_TexCoord92;
			#endif
			float cos44 = cos( degrees( _TextureRotation ) );
			float sin44 = sin( degrees( _TextureRotation ) );
			float2 rotator44 = mul( staticSwitch90 - float2( 0,0 ) , float2x2( cos44 , -sin44 , sin44 , cos44 )) + float2( 0,0 );
			float2 panner12 = ( 1.0 * _Time.y * appendResult13 + rotator44);
			float2 appendResult147 = (float2(_NoiseSpeedX , _NoiseSpeedY));
			float2 appendResult141 = (float2(_Noise_Tiling_X , _Noise_Tiling_Y));
			float2 temp_output_149_0 = ( i.uv_texcoord * appendResult141 );
			float2 appendResult142 = (float2(_Noise_Offset_X , _Noise_Offset_Y));
			float2 uv_TexCoord146 = i.uv_texcoord * temp_output_149_0 + ( temp_output_149_0 + appendResult142 );
			float cos162 = cos( degrees( _NoiseRotation ) );
			float sin162 = sin( degrees( _NoiseRotation ) );
			float2 rotator162 = mul( uv_TexCoord146 - float2( 0,0 ) , float2x2( cos162 , -sin162 , sin162 , cos162 )) + float2( 0,0 );
			float2 panner148 = ( 1.0 * _Time.y * appendResult147 + rotator162);
			float2 appendResult72 = (float2(_NoiseSpeedX , _NoiseSpeedY));
			float2 temp_output_34_0_g1 = ( i.uv_texcoord - float2( 0.5,0.5 ) );
			float2 break39_g1 = temp_output_34_0_g1;
			float2 appendResult50_g1 = (float2(( 1.0 * ( length( temp_output_34_0_g1 ) * 2.0 ) ) , ( ( atan2( break39_g1.x , break39_g1.y ) * ( 1.0 / 6.28318548202515 ) ) * 1.0 )));
			float2 appendResult76 = (float2(_Noise_Tiling_X , _Noise_Tiling_Y));
			float2 temp_output_86_0 = ( appendResult50_g1 * appendResult76 );
			float2 appendResult77 = (float2(_Noise_Offset_X , _Noise_Offset_Y));
			float2 uv_TexCoord75 = i.uv_texcoord * temp_output_86_0 + ( temp_output_86_0 + appendResult77 );
			float cos159 = cos( degrees( _NoiseRotation ) );
			float sin159 = sin( degrees( _NoiseRotation ) );
			float2 rotator159 = mul( uv_TexCoord75 - float2( 0,0 ) , float2x2( cos159 , -sin159 , sin159 , cos159 )) + float2( 0,0 );
			float2 panner71 = ( 1.0 * _Time.y * appendResult72 + rotator159);
			#ifdef _POLARNOISE_ON
				float2 staticSwitch136 = panner71;
			#else
				float2 staticSwitch136 = panner148;
			#endif
			float4 temp_cast_2 = (_Noise_Power).xxxx;
			float4 Noise107 = pow( ( ( floor( ( tex2D( _Noise_Tex, staticSwitch136 ) * _Noise_Steps ) ) / _Noise_Steps ) * _Noise_Intensity ) , temp_cast_2 );
			float4 tex2DNode2 = tex2D( _Main_Tex, ( float4( panner12, 0.0 , 0.0 ) + Noise107 ).rg );
			#ifdef _INVERT_ON
				float4 staticSwitch185 = ( 1.0 - tex2DNode2 );
			#else
				float4 staticSwitch185 = tex2DNode2;
			#endif
			float4 temp_cast_4 = (_Power).xxxx;
			float4 temp_output_7_0 = pow( ( floor( ( staticSwitch185 * _Main_Steps ) ) / _Main_Steps ) , temp_cast_4 );
			float2 appendResult170 = (float2(_Noise02_Speed_X , _Noise02_Speed_Y));
			float2 appendResult171 = (float2(_Noise02_Tiling_X , _Noise02_Tiling_Y));
			float2 uv_TexCoord173 = i.uv_texcoord * appendResult171;
			float2 panner184 = ( 1.0 * _Time.y * appendResult170 + uv_TexCoord173);
			float3 Noise_02179 = ( tex2D( _Noise02_tex, panner184 ).rgb * _Noise02_Intensity );
			#ifdef _USE_NOISE_ON
				float4 staticSwitch182 = ( i.vertexColor * ( temp_output_7_0 * float4( Noise_02179 , 0.0 ) ) );
			#else
				float4 staticSwitch182 = ( i.vertexColor * temp_output_7_0 );
			#endif
			float4 temp_output_52_0 = ( float4( Gradient105 , 0.0 ) + staticSwitch182 );
			#ifdef _CONTROL_EMISSIVE_ON
				float4 staticSwitch134 = ( temp_output_52_0 * i.uv3_texcoord3.z );
			#else
				float4 staticSwitch134 = ( temp_output_52_0 * _Emissive );
			#endif
			o.Emission = staticSwitch134.rgb;
			float4 ase_positionSS = float4( i.screenPos.xyz , i.screenPos.w + 1e-7 );
			float4 ase_positionSSNorm = ase_positionSS / ase_positionSS.w;
			ase_positionSSNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_positionSSNorm.z : ase_positionSSNorm.z * 0.5 + 0.5;
			float screenDepth40 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_positionSSNorm.xy ));
			float distanceDepth40 = abs( ( screenDepth40 - LinearEyeDepth( ase_positionSSNorm.z ) ) / ( _DepthFade ) );
			float temp_output_69_0 = ( i.vertexColor.a * ( tex2DNode2.a * saturate( distanceDepth40 ) ) );
			#ifdef _USE_VERT_COLOR_ON
				float staticSwitch111 = ( pow( i.vertexColor.r , _Vertex_Fade ) * temp_output_69_0 );
			#else
				float staticSwitch111 = temp_output_69_0;
			#endif
			o.Alpha = staticSwitch111;
			clip( tex2DNode2.a - _Cutoff );
		}

		ENDCG
	}
	Fallback Off
	CustomEditor "AmplifyShaderEditor.MaterialInspector"
}
/*ASEBEGIN
Version=19905
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;110;-6453.574,3888;Inherit;False;3321.156;1299.348;Noise;38;154;153;152;151;107;96;97;82;141;150;81;80;160;85;86;163;73;72;161;74;159;71;136;83;70;148;162;146;75;145;87;149;77;76;79;78;142;147;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;78;-6320,4144;Inherit;False;Property;_Noise_Tiling_X;Noise_Tiling_X;28;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;79;-6320,4224;Inherit;False;Property;_Noise_Tiling_Y;Noise_Tiling_Y;29;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;80;-6224,4464;Inherit;False;Property;_Noise_Offset_X;Noise_Offset_X;32;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;81;-6224,4560;Inherit;False;Property;_Noise_Offset_Y;Noise_Offset_Y;33;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;76;-6096,4144;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;85;-6320,3968;Inherit;False;Polar Coordinates;-1;;1;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;3;FLOAT2;0;FLOAT;55;FLOAT;56
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;150;-6192,4816;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;141;-6144,4720;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;77;-5840,4304;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;142;-5712,5024;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;149;-5808,4832;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;86;-5920,4096;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;87;-5744,4128;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;145;-5616,4848;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;161;-5792,4496;Inherit;False;Property;_NoiseRotation;Noise Rotation;26;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;75;-5600,4080;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;146;-5472,4800;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;74;-5792,4576;Inherit;False;Property;_NoiseSpeedY;Noise Speed Y;31;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;73;-5584,4320;Inherit;False;Property;_NoiseSpeedX;Noise Speed X;30;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DegreesOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;163;-5312,4496;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DegreesOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;160;-5600,3984;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;147;-5296,5040;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;162;-5232,4800;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;159;-5312,4080;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;72;-5248,4320;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;148;-5024,4800;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;71;-5104,4080;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;136;-4896,4064;Inherit;False;Property;_PolarNoise;Polar Noise?;23;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;154;-4528,4352;Inherit;False;Property;_Noise_Steps;Noise_Steps;24;0;Create;True;0;0;0;False;0;False;180;180;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;70;-4608,4048;Inherit;True;Property;_Noise_Tex;Noise_Tex;22;1;[Header];Create;True;1;Distortion Noise;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.Vector2Node, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;95;-3776,320;Inherit;False;Property;_Center;Center;4;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;93;-3776,464;Inherit;False;Property;_RadialScale;Radial Scale;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;94;-3776,544;Inherit;False;Property;_LengthScale;Length Scale;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;98;-3776,864;Inherit;False;2;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;48;-3424,752;Inherit;False;Property;_Offset_X;Offset_X;14;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;49;-3424,832;Inherit;False;Property;_Offset_Y;Offset_Y;15;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;151;-4304,4048;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;4;-3424,560;Inherit;False;Property;_Tiling_X;Tiling_X;12;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;5;-3424,640;Inherit;False;Property;_Tiling_Y;Tiling_Y;13;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;88;-3424,304;Inherit;False;Polar Coordinates;-1;;2;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;3;FLOAT2;0;FLOAT;55;FLOAT;56
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;47;-3200,752;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;100;-3200,896;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FloorOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;152;-4112,4048;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;6;-3200,560;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;89;-3040,304;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;99;-3008,752;Inherit;False;Property;_Control_Offset;Control_Offset?;10;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;91;-2832,400;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;83;-4512,4256;Inherit;False;Property;_Noise_Intensity;Noise_Intensity;27;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;153;-3936,4048;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;92;-2656,528;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;3;-2656,288;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;46;-2400,192;Inherit;False;Property;_TextureRotation;Texture Rotation;11;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;82;-3776,4048;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;97;-3728,4320;Inherit;False;Property;_Noise_Power;Noise_Power;25;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;90;-2400,288;Inherit;False;Property;_Polar;Polar?;1;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;14;-2208,560;Inherit;False;Property;_MainSpeedX;Main Speed X;16;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;15;-2208,640;Inherit;False;Property;_MainSpeedY;Main Speed Y;17;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DegreesOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;45;-2160,192;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;96;-3600,4048;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;164;-6432,2000;Inherit;False;2084;466.95;Noise 02;12;177;176;175;173;171;170;168;167;166;165;179;184;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;13;-1984,560;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;44;-2016,368;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;107;-3424,4048;Inherit;False;Noise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;165;-6384,2080;Inherit;False;Property;_Noise02_Tiling_X;Noise02_Tiling_X;44;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;166;-6384,2160;Inherit;False;Property;_Noise02_Tiling_Y;Noise02_Tiling_Y;45;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;12;-1792,368;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;108;-1776,512;Inherit;False;107;Noise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;171;-6176,2080;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;167;-6016,2240;Inherit;False;Property;_Noise02_Speed_X;Noise02_Speed_X;46;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;168;-6016,2320;Inherit;False;Property;_Noise02_Speed_Y;Noise02_Speed_Y;47;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;84;-1536,368;Inherit;False;2;2;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;109;-6432,2528;Inherit;False;2084;466.95;Gradient;14;63;64;65;62;67;68;59;61;60;55;56;66;58;105;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;173;-5872,2080;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;170;-5808,2240;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;2;-1376,320;Inherit;True;Property;_Main_Tex;Main_Tex;5;1;[Header];Create;True;0;0;0;False;0;False;-1;None;5643197f0c9afc54998735daa55c5f68;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.OneMinusNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;186;-1072,256;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;63;-6384,2608;Inherit;False;Property;_Gradient_Tiling_X;Gradient_Tiling_X;37;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;64;-6384,2688;Inherit;False;Property;_Gradient_Tiling_Y;Gradient_Tiling_Y;38;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;65;-6384,2800;Inherit;False;Property;_Gradient_Offset_X;Gradient_Offset_X;39;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;66;-6384,2880;Inherit;False;Property;_Gradient_Offset_Y;Gradient_Offset_Y;40;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;184;-5424,2080;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;158;-1280,544;Inherit;False;Property;_Main_Steps;Main_Steps;6;0;Create;True;0;0;0;False;0;False;180;180;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;185;-944,144;Inherit;False;Property;_Invert;Invert?;7;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;62;-5872,2752;Inherit;False;Property;_GradientRotation;Gradient Rotation;35;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;67;-6176,2800;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;68;-6176,2608;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;176;-5088,2288;Inherit;False;Property;_Noise02_Intensity;Noise02_Intensity;43;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;175;-5152,2064;Inherit;True;Property;_Noise02_tex;Noise02_tex;41;1;[Header];Create;True;1;Noise;0;0;False;0;False;-1;None;0341c350a6f1efa40bd348f68ea999c9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;155;-928,352;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;112;-6448,3104;Inherit;False;2441.716;681.5266;WPO;20;128;127;126;125;124;123;130;117;119;122;121;120;118;116;115;114;113;132;131;133;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DegreesOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;59;-5616,2688;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;61;-5872,2608;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;177;-4768,2080;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FloorOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;156;-752,352;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;132;-6416,3280;Inherit;False;Property;_WPO_X_Tiling;WPO_X_Tiling;49;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;131;-6416,3360;Inherit;False;Property;_WPO_Y_Tiling;WPO_Y_Tiling;50;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;60;-5408,2608;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;179;-4576,2080;Inherit;False;Noise_02;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;157;-576,352;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;8;-640,544;Inherit;False;Property;_Power;Power;18;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;113;-5968,3456;Inherit;False;Property;_WPO_V_Speed;WPO_V_Speed;57;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;114;-5968,3376;Inherit;False;Property;_WPO_U_Speed;WPO_U_Speed;55;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;133;-6160,3296;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;58;-5088,2816;Inherit;False;Property;_Gradient_Intensity;Gradient_Intensity;36;0;Create;True;0;0;0;False;0;False;0;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;55;-5152,2576;Inherit;True;Property;_Gradient_Tex;Gradient_Tex;34;1;[Header];Create;True;1;Gradient;0;0;False;0;False;-1;None;549da80cd7da5b0418238f050300eb90;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;181;-432,592;Inherit;False;179;Noise_02;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;41;-704,1008;Inherit;False;Property;_DepthFade;Depth Fade;21;0;Create;True;0;0;0;False;0;False;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;7;-400,352;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;115;-5792,3184;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;116;-5712,3392;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;56;-4768,2608;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VertexColorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;37;-400,160;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;180;-256,496;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;40;-416,992;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;118;-4864,3392;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;120;-4688,3536;Inherit;False;Property;_World_or_Vertex;World_or_Vertex;53;1;[Enum];Create;True;0;2;World;0;Vertex;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;121;-4864,3536;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;122;-5520,3216;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;119;-5360,3600;Inherit;False;Property;_WWPO_SC;WWPO_S/C;56;1;[Enum];Create;True;0;2;OFF;0;ON;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;117;-5360,3408;Inherit;False;Property;_WPO_power;WPO_power;52;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;130;-5360,3504;Inherit;False;Property;_WPO_power_02;WPO_power_02;54;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;105;-4592,2608;Inherit;False;Gradient;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;36;-48,256;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;183;-48,368;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;43;-160,992;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;103;-400,64;Inherit;False;Property;_Vertex_Fade;Vertex_Fade;9;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;124;-4672,3392;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector4Node, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;125;-4480,3424;Inherit;False;Property;_WPO_Direction;WPO_Direction;51;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;126;-5120,3424;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;123;-5248,3184;Inherit;True;Property;_WPO_tex;WPO_tex;48;1;[Header];Create;True;1;WPO;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;106;56,144;Inherit;False;105;Gradient;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;182;96,336;Inherit;False;Property;_Use_Noise;Use_Noise?;42;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;42;72,800;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;102;-192,96;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;127;-4512,3216;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;52;264,240;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;39;264,144;Inherit;False;Property;_Emissive;Emissive;19;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;188;-6432,1488;Inherit;False;2084;466.95;Mask;12;199;197;196;195;194;193;192;191;190;189;200;201;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;69;280,576;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;128;-4240,3216;Inherit;False;WPO;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;38;424,240;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;135;424,368;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;104;464,496;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;189;-6384,1568;Inherit;False;Property;_Mask_Tiling_X;Mask_Tiling_X;60;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;190;-6384,1648;Inherit;False;Property;_Mask_Tiling_Y;Mask_Tiling_Y;61;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;191;-6176,1568;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;192;-6016,1728;Inherit;False;Property;_Mask_Speed_X;Mask_Speed_X;62;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;193;-6016,1808;Inherit;False;Property;_Mask_Speed_Y;Mask_Speed_Y;63;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;194;-5872,1568;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;195;-5808,1728;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;196;-5424,1568;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;200;-5152,1552;Inherit;True;Property;_Mask_tex;Mask_tex;58;0;Create;True;0;0;0;False;0;False;-1;None;845ccdae4e1121746949d88d13f712e2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;197;-5056,1760;Inherit;False;Property;_Mask_Intensity;Mask_Intensity;59;0;Create;True;0;0;0;False;0;False;1;12.36;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;199;-4784,1552;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;201;-4592,1552;Inherit;False;Mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;111;680,576;Inherit;False;Property;_Use_Vert_Color;Use_Vert_Color?;8;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;129;760,416;Inherit;False;128;WPO;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StaticSwitch, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;134;632,240;Inherit;False;Property;_Control_Emissive;Control_Emissive?;20;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;1;1232,208;Float;False;True;-1;3;AmplifyShaderEditor.MaterialInspector;0;0;Unlit;AS_Particles;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0;True;False;0;True;Custom;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;2;5;False;;10;False;;0;1;False;;1;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;76;0;78;0
WireConnection;76;1;79;0
WireConnection;141;0;78;0
WireConnection;141;1;79;0
WireConnection;77;0;80;0
WireConnection;77;1;81;0
WireConnection;142;0;80;0
WireConnection;142;1;81;0
WireConnection;149;0;150;0
WireConnection;149;1;141;0
WireConnection;86;0;85;0
WireConnection;86;1;76;0
WireConnection;87;0;86;0
WireConnection;87;1;77;0
WireConnection;145;0;149;0
WireConnection;145;1;142;0
WireConnection;75;0;86;0
WireConnection;75;1;87;0
WireConnection;146;0;149;0
WireConnection;146;1;145;0
WireConnection;163;0;161;0
WireConnection;160;0;161;0
WireConnection;147;0;73;0
WireConnection;147;1;74;0
WireConnection;162;0;146;0
WireConnection;162;2;163;0
WireConnection;159;0;75;0
WireConnection;159;2;160;0
WireConnection;72;0;73;0
WireConnection;72;1;74;0
WireConnection;148;0;162;0
WireConnection;148;2;147;0
WireConnection;71;0;159;0
WireConnection;71;2;72;0
WireConnection;136;1;148;0
WireConnection;136;0;71;0
WireConnection;70;1;136;0
WireConnection;151;0;70;0
WireConnection;151;1;154;0
WireConnection;88;2;95;0
WireConnection;88;3;93;0
WireConnection;88;4;94;0
WireConnection;47;0;48;0
WireConnection;47;1;49;0
WireConnection;100;0;98;1
WireConnection;100;1;98;2
WireConnection;152;0;151;0
WireConnection;6;0;4;0
WireConnection;6;1;5;0
WireConnection;89;0;88;0
WireConnection;89;1;6;0
WireConnection;99;1;47;0
WireConnection;99;0;100;0
WireConnection;91;0;89;0
WireConnection;91;1;47;0
WireConnection;153;0;152;0
WireConnection;153;1;154;0
WireConnection;92;0;6;0
WireConnection;92;1;99;0
WireConnection;3;0;89;0
WireConnection;3;1;91;0
WireConnection;82;0;153;0
WireConnection;82;1;83;0
WireConnection;90;1;92;0
WireConnection;90;0;3;0
WireConnection;45;0;46;0
WireConnection;96;0;82;0
WireConnection;96;1;97;0
WireConnection;13;0;14;0
WireConnection;13;1;15;0
WireConnection;44;0;90;0
WireConnection;44;2;45;0
WireConnection;107;0;96;0
WireConnection;12;0;44;0
WireConnection;12;2;13;0
WireConnection;171;0;165;0
WireConnection;171;1;166;0
WireConnection;84;0;12;0
WireConnection;84;1;108;0
WireConnection;173;0;171;0
WireConnection;170;0;167;0
WireConnection;170;1;168;0
WireConnection;2;1;84;0
WireConnection;186;0;2;0
WireConnection;184;0;173;0
WireConnection;184;2;170;0
WireConnection;185;1;2;0
WireConnection;185;0;186;0
WireConnection;67;0;65;0
WireConnection;67;1;66;0
WireConnection;68;0;63;0
WireConnection;68;1;64;0
WireConnection;175;1;184;0
WireConnection;155;0;185;0
WireConnection;155;1;158;0
WireConnection;59;0;62;0
WireConnection;61;0;68;0
WireConnection;61;1;67;0
WireConnection;177;0;175;5
WireConnection;177;1;176;0
WireConnection;156;0;155;0
WireConnection;60;0;61;0
WireConnection;60;2;59;0
WireConnection;179;0;177;0
WireConnection;157;0;156;0
WireConnection;157;1;158;0
WireConnection;133;0;132;0
WireConnection;133;1;131;0
WireConnection;55;1;60;0
WireConnection;7;0;157;0
WireConnection;7;1;8;0
WireConnection;115;0;133;0
WireConnection;116;0;114;0
WireConnection;116;1;113;0
WireConnection;56;0;55;5
WireConnection;56;1;58;0
WireConnection;180;0;7;0
WireConnection;180;1;181;0
WireConnection;40;0;41;0
WireConnection;122;0;115;0
WireConnection;122;2;116;0
WireConnection;105;0;56;0
WireConnection;36;0;37;0
WireConnection;36;1;180;0
WireConnection;183;0;37;0
WireConnection;183;1;7;0
WireConnection;43;0;40;0
WireConnection;124;0;118;0
WireConnection;124;1;121;0
WireConnection;124;2;120;0
WireConnection;126;0;117;0
WireConnection;126;1;130;0
WireConnection;126;2;119;0
WireConnection;123;1;122;0
WireConnection;182;1;183;0
WireConnection;182;0;36;0
WireConnection;42;0;2;4
WireConnection;42;1;43;0
WireConnection;102;0;37;1
WireConnection;102;1;103;0
WireConnection;127;0;123;1
WireConnection;127;1;126;0
WireConnection;127;2;124;0
WireConnection;127;3;125;0
WireConnection;52;0;106;0
WireConnection;52;1;182;0
WireConnection;69;0;37;4
WireConnection;69;1;42;0
WireConnection;128;0;127;0
WireConnection;38;0;52;0
WireConnection;38;1;39;0
WireConnection;135;0;52;0
WireConnection;135;1;98;3
WireConnection;104;0;102;0
WireConnection;104;1;69;0
WireConnection;191;0;189;0
WireConnection;191;1;190;0
WireConnection;194;0;191;0
WireConnection;195;0;192;0
WireConnection;195;1;193;0
WireConnection;196;0;194;0
WireConnection;196;2;195;0
WireConnection;200;1;196;0
WireConnection;199;0;200;1
WireConnection;199;1;197;0
WireConnection;201;0;199;0
WireConnection;111;1;69;0
WireConnection;111;0;104;0
WireConnection;134;1;38;0
WireConnection;134;0;135;0
WireConnection;1;2;134;0
WireConnection;1;9;111;0
WireConnection;1;10;2;4
WireConnection;1;11;129;0
ASEEND*/
//CHKSM=AFB779356C06C8B8051846FEAE83B880A7515D1C