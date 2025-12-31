Shader "Bloody Botany/UI/Gradient Scroll"
{
    Properties
    {
        [HideInInspector] _MainTex ("Main Tex", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        [SingleLineTexture] _GradientTex ("Gradient", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", Range(0, 5)) = 4
        _Rotation ("Rotation", Float) = 0
        [HideInInspector][Toggle] _Stencil ("Stencil", Float) = 0
        [HideInInspector][Toggle] _StencilOp ("StencilOp", Float) = 0
        [HideInInspector][Toggle] _StencilComp ("StencilComp", Float) = 0
        [HideInInspector][Toggle] _StencilReadMask ("StencilReadMask", Float) = 0
        [HideInInspector][Toggle] _StencilWriteMask ("StencilWriteMask", Float) = 0
        [HideInInspector][Toggle] _ColorMask ("ColorMask", Float) = 0
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
            sampler2D _GradientTex;
            half _ScrollSpeed;
            half _Rotation;

            //float2x2 rotationMat = float2x2(cos(-45), -sin(-45), sin(-45), cos(-45));

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
                half angleC = cos(_Rotation);
                half angleS = sin(_Rotation);
                float2x2 rotationMat = float2x2(angleC, -angleS, angleS, angleC);

                half2 uv = mul(i.uv, rotationMat);
                half4 tex = tex2D(_MainTex, i.uv);
                half3 grad = tex2D(_GradientTex, (uv + _Time.y * _ScrollSpeed)).rgb;

                return float4(tex.rgb * grad, tex.a);
            }
            ENDCG
        }
    }
}
