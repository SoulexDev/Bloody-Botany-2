Shader "Unlit/CrosshairCircle"
{
    Properties
    {
        [HideInInspector] _MainTex ("Main Tex", 2D) = "white" {}
        _CircleRadius ("Circle Radius", Range(0, 0.5)) = 0.25
        _CircleThickness ("Circle Thickness", Range(0.01, 1)) = 0.025
        _OutlineThickness ("Outline Thickness", Range(0, 1)) = 0.025

        _InnerCircleRadius ("Inner Circle Radius", Range(0, 0.25)) = 0.1

        [Toggle] _UseCrosshairOuter ("Use Crosshair Outer", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            half _CircleRadius;
            half _CircleThickness;
            half _OutlineThickness;

            half _InnerCircleRadius;
            half _UseCrosshairOuter;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half dist = distance(i.uv, half2(0.5, 0.5));

                half ring_inner = step(dist, _CircleRadius);
                half ring_outer = step(dist, _CircleRadius + _CircleThickness);

                half outline_ring_inner = step(dist, _CircleRadius - _OutlineThickness);
                half outline_ring_outer = step(dist, _CircleRadius + _CircleThickness + _OutlineThickness);

                half circle = ring_outer - ring_inner;
                half outline = outline_ring_outer - outline_ring_inner;

                half circle_two = step(dist, _InnerCircleRadius);
                half outline_two = step(dist, _InnerCircleRadius + _OutlineThickness);

                if (max(outline * _UseCrosshairOuter, outline_two) <= 0.0) discard;

                half final = circle * _UseCrosshairOuter + circle_two;

                return float4(final, final, final, 1);
            }
            ENDCG
        }
    }
}
