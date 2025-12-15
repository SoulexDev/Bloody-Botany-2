Shader "Bloody Botany/BlendedGrass"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _TopColor ("Top Color (Temp)", Color) = (1, 1, 1, 1)

        [NoScaleOffset][SingleLineTexture] _MainTex ("Albedo", 2D) = "white" {}
        [NoScaleOffset][SingleLineTexture] _NormalMap ("Normal Map", 2D) = "bump" {}
        [NoScaleOffset][SingleLineTexture] _ORM ("ORM Map", 2D) = "white" {}

        [NoScaleOffset][SingleLineTexture] _WindTex ("Wind Texture", 2D) = "black" {}
        _WindTexScale ("Wind Texture Scale", Float) = 0.25
        _WindSpeed ("Wind Speed", Float) = 1
        _WindStrength ("Wind Strength", Float) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Cull Off
        ZWrite On

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 4.0

        half4 _Color;
        half4 _TopColor;
        sampler2D _MainTex;
        sampler2D _NormalMap;
        sampler2D _ORM;
        sampler2D _WindTex;
        half _WindTexScale;
        half _WindSpeed;
        half _WindStrength;

        half _OrthoScale;
        half _OffsetX;
        half _OffsetZ;
        sampler2D _TerrainTex;
        
        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert (inout appdata_full v)
        {
            //UNITY_INITIALIZE_OUTPUT(Input, o);

            float3 worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);

            half4 wind = tex2Dlod(_WindTex, float4(worldPos.xz * _WindTexScale + _Time.x * _WindSpeed, 0, 0));

            wind *= 2;
            wind -= 1;

            v.vertex.xz += half2(wind.r, wind.g) * _WindStrength * v.vertex.y;
        }
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            half2 terrainUV = (IN.worldPos.xz - half2(_OffsetX, _OffsetZ)) / _OrthoScale;

            half4 albedo = tex2D(_MainTex, IN.uv_MainTex);
            half3 terrainColor = tex2D(_TerrainTex, terrainUV);

            half interp = saturate(IN.uv_MainTex.y * 2 - 0.25);

            half3 orm = tex2D(_ORM, IN.uv_MainTex).rgb;

            o.Albedo = lerp(0, albedo.rgb, interp);
            o.Smoothness = lerp(0, 1 - orm.g, interp);
            o.Metallic = lerp(0, orm.r, interp);
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
            o.Occlusion = lerp(1, orm.b, interp);
            o.Emission = lerp(terrainColor, 0, interp);

            clip(albedo.a - 0.5);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
