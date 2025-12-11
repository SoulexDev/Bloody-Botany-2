Shader "Bloody Botany/RetroStandard"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        [NoScaleOffset][SingleLineTexture] _NoiseTex ("Noise", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Float) = 0.25
        [NoScaleOffset][SingleLineTexture] _MainTex ("Albedo", 2D) = "white" {}
        [NoScaleOffset][SingleLineTexture] _NormalMap ("Normal Map", 2D) = "bump" {}
        //_NormalScale ("Normal Scale", Range(0.0, 1.0)) = 1.0
        [NoScaleOffset][SingleLineTexture] _MetallicMap ("Metallic Map", 2D) = "white" {}
        _Metallic ("Metallic", Range(0.0, 1.0)) = 0.0
        [NoScaleOffset][SingleLineTexture] _RoughnessMap ("Roughness Map", 2D) ="white" {}
        _Roughness ("Roughness", Range(0.1, 1.0)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 4.0

        float4 _Color;
        sampler2D _NoiseTex;
        half _NoiseScale;
        sampler2D _MainTex;
        sampler2D _NormalMap;
        //half _NormalScale;
        sampler2D _MetallicMap;
        half _Metallic;
        sampler2D _RoughnessMap;
        half _Roughness;

        struct Input
        {
            float2 uv_MainTex;
            float3 color : COLOR;
        };
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            half4 noise = tex2D(_NoiseTex, IN.uv_MainTex * _NoiseScale);

            o.Albedo = albedo * IN.color * noise.r;
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
            o.Metallic = tex2D(_MetallicMap, IN.uv_MainTex) * _Metallic;
            o.Smoothness = 1 - tex2D(_RoughnessMap, IN.uv_MainTex) * _Roughness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
