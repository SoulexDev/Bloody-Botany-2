Shader "Bloody Botany/Window"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        [NoScaleOffset][SingleLineTexture] _MainTex ("Albedo", 2D) = "white" {}
        [NoScaleOffset][SingleLineTexture] _NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalScale ("Normal Scale", Range(0.0, 1.0)) = 1.0
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
        sampler2D _MainTex;
        sampler2D _NormalMap;
        half _NormalScale;
        sampler2D _MetallicMap;
        half _Metallic;
        sampler2D _RoughnessMap;
        half _Roughness;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 worldNormal;
            float3 color : COLOR;

            INTERNAL_DATA
        };
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex) * _NormalScale);
            o.Metallic = tex2D(_MetallicMap, IN.uv_MainTex) * _Metallic;
            o.Smoothness = 1 - tex2D(_RoughnessMap, IN.uv_MainTex) * _Roughness;

            float3 worldNormal = WorldNormalVector(IN, o.Normal);

            half3 lightDir = normalize(_WorldSpaceLightPos0.xyz - IN.worldPos * _WorldSpaceLightPos0.w);
            half NdotL = pow(dot(worldNormal, -lightDir) * 0.5 + 1, 4);

            o.Emission = NdotL * _LightColor0.rgb * 0.2 * (1.0 - IN.color.r * 0.5);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
