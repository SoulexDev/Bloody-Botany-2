Shader "Bloody Botany/TrimStandard"
{
    Properties
    {
        [Header(Material Properties)]
        [Space]
        [SingleLineTexture] _MainTex ("Albedo", 2D) = "white" {}
        [SingleLineTexture][Normal] _NormalMap ("Normal Map", 2D) = "bump" {}
        [SingleLineTexture] _SpecMap ("Specular Map", 2D) = "black" {}
        [SingleLineTexture] _GlossMap ("Glossiness Map", 2D) = "white" {}
        _Glossiness ("Glossiness", Range(0.0, 1.0)) = 0.5
        //_Contrast ("Contrast", Range(0, 4)) = 1

        [Header(Masks)]
        [Space]
        [SingleLineTexture] _BrushMask ("Brush Strokes Mask", 2D) = "black" {}
        [SingleLineTexture] _ProcMask ("Procedural Mask", 2D) = "black" {}

        [Header(Colors)]
        [Space]
        _BaseTint ("Base Tint", Color) = (1,1,1,1)
        _MaskColor0 ("Mask Color 0", Color) = (0.5, 0.5, 0.5, 1)
        _MaskColor1 ("Mask Color 1", Color) = (0.5, 0.5, 0.5, 1)
        _MaskColor2 ("Mask Color 2", Color) = (0.5, 0.5, 0.5, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf StandardSpecular fullforwardshadows
        #pragma target 4.0

        sampler2D _MainTex;
        sampler2D _NormalMap;
        sampler2D _SpecMap;
        sampler2D _GlossMap;
        half _Glossiness;
        //half _Contrast;

        sampler2D _BrushMask;
        sampler2D _ProcMask;

        half4 _BaseTint;
        half4 _MaskColor0;
        half4 _MaskColor1;
        half4 _MaskColor2;

        struct Input
        {
            float2 uv_MainTex;
            float4 color : COLOR;
            float3 worldNormal;
            float3 worldPos;

            INTERNAL_DATA
        };
        void ApplyMasksToWeights(half3 masks, inout half3 weights, float height_blend_range){
	        masks *= weights;

	        half mask_start = max(max(masks.r, masks.g), masks.b) - height_blend_range;

	        masks = max(masks - mask_start, 0.0);

	        weights.rgb = masks/dot(masks, 1.0);
        }
        void Contrast(inout half3 value, half contrast){
            half midPoint = pow(0.5, 2.2);
            value = (value - midPoint) * contrast + midPoint;
            value = saturate(value);
        }
        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            //Calculate and assign normal
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex)).rgb;
            half3 worldNormal = WorldNormalVector(IN, o.Normal);
            

            //Create triplanar mask
            half3 triplanarMask = abs(worldNormal);
            triplanarMask = pow(triplanarMask, 8);
            triplanarMask /= (triplanarMask.x + triplanarMask.y + triplanarMask.z);

            //Create triplanar texture
            half3 procMaskX = tex2D(_ProcMask, IN.worldPos.yz / 64) * triplanarMask.r;
            half3 procMaskY = tex2D(_ProcMask, IN.worldPos.xz / 64) * triplanarMask.g;
            half3 procMaskZ = tex2D(_ProcMask, IN.worldPos.xy / 64) * triplanarMask.b;
            half3 procMask = procMaskX + procMaskY + procMaskZ;

            //Add vertex color and mask out unwanted areas
            procMask += IN.color.rgb;
            procMask = saturate(procMask);
            procMask *= IN.color.a;

            //Mask strokes
            half3 strokesMask = tex2D(_BrushMask, IN.uv_MainTex).rgb * procMask;
            Contrast(strokesMask, 4);
            
            //Sample albedo and overlay strokes
            half3 albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _BaseTint.rgb;
            albedo = lerp(albedo, _MaskColor0, strokesMask.r * _MaskColor0.a);
            albedo = lerp(albedo, _MaskColor1, strokesMask.g * _MaskColor1.a);
            albedo = lerp(albedo, _MaskColor2, strokesMask.b * _MaskColor2.a);

            half3 spec = tex2D(_MainTex, IN.uv_MainTex).rgb + strokesMask * 0.5 * half3(_MaskColor0.a, _MaskColor1.a, _MaskColor2.a);
            half gloss = tex2D(_GlossMap, IN.uv_MainTex).r * _Glossiness;

            o.Albedo = albedo;
            o.Specular = spec;
            o.Smoothness = gloss;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
