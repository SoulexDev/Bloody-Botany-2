Shader "Bloody Botany/UI/Blur"
{
    Properties
    {
        [HideInInspector] _MainTex ("Main Tex", 2D) = "white" {}
        //_Color ("Color", Color) = (1, 1, 1, 1)
        _SceneColor ("Scene Color", 2D) = "white" {}
        _BlurAmount ("Blur Amount", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

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
                float4 screenPos : TEXCOORD1;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _SceneColor;
            half _BlurAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half4 tex = tex2D(_MainTex, i.uv);
                half blur = _BlurAmount * 4;

                half blurLower = floor(blur);
                half blurUpper = ceil(blur);

                half blend = frac(blur);

                half4 sceneColLower = tex2Dlod(_SceneColor, float4(i.screenPos.xy / i.screenPos.w, 0, blurLower));
                half4 sceneColUpper = tex2Dlod(_SceneColor, float4(i.screenPos.xy / i.screenPos.w, 0, blurUpper));

                half4 sceneCol = lerp(sceneColLower, sceneColUpper, blend);

                half alpha = i.color.a * tex.a;
                half3 color = lerp(sceneCol.rgb, tex.rgb, alpha) * i.color.rgb;
                return float4(color, 1);
            }
            ENDCG
        }
    }
}
