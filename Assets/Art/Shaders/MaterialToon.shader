Shader "Bloody Botany/MaterialToon"
{
    Properties
    {
        _MainTex ("Layer Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Color0 ("Layer Color 0", Color) = (1,1,1,1)
        _Color1 ("Layer Color 2", Color) = (0,0,0,1)
        _Roughness ("Roughness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        sampler2D _NormalMap;
        half4 _Color0;
        half4 _Color1;
        half _Roughness;
        half _Metallic;

        //UNITY_INSTANCING_BUFFER_START(Props)
        //UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = lerp(_Color0, _Color1, tex2D(_MainTex, IN.uv_MainTex));
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
            o.Smoothness = 1 - _Roughness;
            o.Metallic = _Metallic;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
