Shader "Bloody Botany/Diagonal Lines"
{
    Properties
    {
        [HideInInspector] _MainTex ("Main Tex", 2D) = "white" {}
        _Color ("Color", Vector) = (1, 1, 1, 1)
        _LineThickness ("Line Thickness", Range(0, 1)) = 0.25
        _ScrollSpeed ("Scroll Speed", Range(0, 5)) = 4
        _OtherLinesValue ("Other Lines Value", Range(0, 1)) = 0.5
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
                float3 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            half4 _Color;
            half _LineThickness;
            half _ScrollSpeed;
            half _OtherLinesValue;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.uv.y += 1;
                i.uv.x += _Time.x * _ScrollSpeed;

                half dist = distance(i.uv, half2(i.uv.y, i.uv.x));

                dist = step(frac(dist / _LineThickness), 0.5);

                dist = max(_OtherLinesValue, dist);

                half3 final = tex2D(_MainTex, i.uv) * dist * i.color;
                return float4(final, 1);
            }
            ENDCG
        }
    }
}
