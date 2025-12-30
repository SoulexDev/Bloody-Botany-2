Shader "Custom/DevTriplanar"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 4.0

        sampler2D _MainTex;
        half4 _Color;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 worldNormal;

            INTERNAL_DATA
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            half3 weights = pow(abs(IN.worldNormal), 8);
            weights = weights / (weights.x + weights.y + weights.z);

            half3 color = 
            tex2D(_MainTex, IN.worldPos.yz) * weights.x + 
            tex2D(_MainTex, IN.worldPos.xz) * weights.y + 
            tex2D(_MainTex, IN.worldPos.xy) * weights.z;
            o.Albedo = color * _Color;
        }
        ENDCG
    }
    FallBack "Diffuse"
}