Shader "Bloody Botany/BeautifulSkybox"
{
    Properties
    {
        [Header(Textures)]
        [SingleLineTexture] _CloudsTex ("Clouds Texture", 2D) = "black" {}
        [SingleLineTexture] _SampleNoise ("Sample Noise", 2D) = "black" {}

        [Header(Sky Color)]
        _TopColor ("Top Color", Color) = (0.5, 0.5, 1, 1)
        _HorizonColor ("Horizon Color", Color) = (1, 0.5, 0.5, 1)
        _BottomColor ("Bottom Color", Color) = (0, 0, 0, 1)

        [Header(Sun Settings)]
        _SunSize ("Sun Size", Range(0, 1)) = 0.1
        _SunSpread ("Sun Spread", Range(0, 1)) = 0.01

        [Header(Ray March Settings)]
        _StepSize ("Ray Step Size", Float) = 0.5
        _MaxAccumSteps ("Max Accumulation Steps", Integer) = 8

        [Header(Cloud Lighting Settings)]
        _AccumIntensity ("Accumulation Intensity", Range(1, 32)) = 16
        _AbsorptionCoefficient ("Absorption Coefficient", Range(0, 16)) = 0.1
        _ScatteringAniso ("Scattering Aniso", Range(0, 4)) = 0.1
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

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
            };

            sampler2D _CloudsTex;
            sampler2D _SampleNoise;

            half3 _TopColor;
            half3 _HorizonColor;
            half3 _BottomColor;

            half _SunSize;
            half _SunSpread;

            half _StepSize;
            int _MaxAccumSteps;

            half _AccumIntensity;
            half _AbsorptionCoefficient;
            half _ScatteringAniso;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.viewDir = o.worldPos - _WorldSpaceCameraPos.xyz;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }
            float henyeyGreenstein(float g, float mu) {
                float gg = g * g;
	            return (1.0 / (4.0 * UNITY_PI))  * ((1.0 - gg) / pow(1.0 + gg - 2.0 * g * mu, 1.5));
            }
            fixed4 frag (v2f i) : SV_Target
            {
                if (_WorldSpaceLightPos0.w > 0){
                    return 0;
                }
                //Normalize view direction;
                half viewLength = length(i.viewDir);
                half3 viewRay = i.viewDir / viewLength;

                //Sample blue noise
                half2 normalizedScreenUV = i.screenPos.xy / i.screenPos.w;
                normalizedScreenUV.x *= (_ScreenParams.x / _ScreenParams.y);
                half noise = tex2D(_SampleNoise, normalizedScreenUV) * 16;
                noise = frac(noise + half(_Time.y % 8) / sqrt(0.5)) * 0.02;

                //Compute sky plane UVs
                half rayLength = 1.0 / dot(viewRay, half3(0, 1, 0)) + noise;
                half3 intersect = viewRay * rayLength;
                half2 topUv = half2(intersect.x, 1 - intersect.z) * 0.25;

                //Create light transport variables
                half2 sunDirTop = _WorldSpaceLightPos0.xz;
                half cloudAccum = 0;
                
                //March through cloud density
                for (int i = 0; i < _MaxAccumSteps; i++){
                    cloudAccum += 1 - tex2Dlod(_CloudsTex, half4(topUv + sunDirTop * (half(i) / _MaxAccumSteps) * _StepSize, 0, 0)).a;
                }

                //Adjust cloud accum
                cloudAccum /= _MaxAccumSteps;
                cloudAccum *= _AccumIntensity;

                //Sample cloud shape
                half cloudShape = tex2D(_CloudsTex, topUv).a;
                cloudShape *= saturate(viewRay.y);

                //Calculate cloud color
                half phase = henyeyGreenstein(_ScatteringAniso, dot(viewRay, _WorldSpaceLightPos0.xyz));
                half beersLaw = exp(-cloudShape * _AbsorptionCoefficient);
                half lightEnergy = beersLaw * cloudAccum * phase;
                half3 cloudColor = lightEnergy * _LightColor0.rgb;

                //Calculate sun color
                half sunDot = dot(viewRay, _WorldSpaceLightPos0.xyz) * 0.5 + 0.5;
                sunDot = 1 - smoothstep(_SunSize, _SunSize + _SunSpread, 1 - sunDot);
                sunDot *= (1 - cloudShape);
                sunDot = pow(sunDot, 4);

                //Calculate sky color
                half3 skyColor = lerp(_HorizonColor, _TopColor, saturate(viewRay.y));
                skyColor = lerp(skyColor, _BottomColor, saturate(-viewRay.y));
                skyColor += sunDot;

                //Finalize color
                half3 color = lerp(skyColor, cloudColor, cloudShape);
                return float4(color, 1);
            }
            ENDCG
        }
    }
}
