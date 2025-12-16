Shader "Bloody Botany/SS Standard"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        [NoScaleOffset][SingleLineTexture] _ThicknessMap ("Thickness Map", 2D) = "black" {}
        [NoScaleOffset][SingleLineTexture] _MainTex ("Albedo", 2D) = "white" {}
        [NoScaleOffset][SingleLineTexture] _NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalScale ("Normal Scale", Range(0.0, 1.0)) = 1.0
        [NoScaleOffset][SingleLineTexture] _MetallicMap ("Metallic Map", 2D) = "white" {}
        _Metallic ("Metallic", Range(0.0, 1.0)) = 0.0
        [NoScaleOffset][SingleLineTexture] _RoughnessMap ("Roughness Map", 2D) ="white" {}
        _Roughness ("Roughness", Range(0.1, 1.0)) = 0.5
        [NoScaleOffset][SingleLineTexture] _EmissionMap ("Emission Map", 2D) = "black" {}

        _FlashBaseColor ("Flash Base Color", Color) = (0, 0, 0, 0)
        _Flash ("Flash", Range(0.0, 1.0)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 4.0

        float4 _Color;
        sampler2D _ThicknessMap;
        sampler2D _MainTex;
        sampler2D _NormalMap;
        half _NormalScale;
        sampler2D _MetallicMap;
        half _Metallic;
        sampler2D _RoughnessMap;
        half _Roughness;
        sampler2D _EmissionMap;

        half3 _FlashBaseColor;
        half _Flash;

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

            half thickness = 1 - tex2D(_ThicknessMap, IN.uv_MainTex).r;

            float3 worldNormal = WorldNormalVector(IN, o.Normal);

            half3 lightDir = normalize(_WorldSpaceLightPos0.xyz - IN.worldPos * _WorldSpaceLightPos0.w);
            half3 viewDir = normalize(IN.worldPos - _WorldSpaceCameraPos.xyz);

            half fresnel = saturate(pow(1 + dot(viewDir, worldNormal), 3));
            half NdotL = saturate(dot(worldNormal, -lightDir) * 0.5 + 0.5);

            half3 flashColor = lerp(lerp(0, _FlashBaseColor, 1 - pow(_Flash - 1, 4)), 1, _Flash);

            o.Emission = tex2D(_EmissionMap, IN.uv_MainTex) + 
            step(1 - fresnel, 0.5) * half3(1, 0, 0) + 
            thickness * fresnel * NdotL * _LightColor0.rgb + flashColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
