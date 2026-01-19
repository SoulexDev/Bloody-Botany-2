Shader "Bloody Botany/Clouds"
{
    Properties
    {
        [HideInInspector] _MainTex ("Main Tex", 2D) = "white" {}
        _BlueNoise ("Blue Noise", 2D) = "black" {}
        _DensityNoise ("Density Noise", 2D) = "white" {}
        _CloudShapeNoise ("Clouds Noise", 3D) = "black" {}
        _MaxRaySteps ("Max ray steps", Integer) = 100
        _MaxLightSteps ("Max light steps", Integer) = 6
        _RayStepSize ("Ray step size", Float) = 0.1
        _AbsorptionCoefficient ("Absorption coefficient", Float) = 0.9
        _ScatteringAniso ("Scattering aniso", Float) = 0.3

        [HideInInspector] _BoundsMin ("", vector) = (0, 0, 0, 0)
        [HideInInspector] _BoundsMax ("", vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        //Blend SrcAlpha OneMinusSrcAlpha
        //LOD 100
        //Cull Front

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
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
            };

            sampler2D _CameraDepthTexture;
            sampler2D _MainTex;
            sampler2D _BlueNoise;
            Texture2D<float4> _DensityNoise;
            SamplerState sampler_DensityNoise;

            Texture3D<float4> _CloudShapeNoise;
            Texture3D<float4> _CloudDetailNoise;
            SamplerState sampler_CloudShapeNoise;
            SamplerState sampler_CloudDetailNoise;

            half _DensityMultiplier;
            half _DensityThreshold;

            half _CloudScale;
            half _DetailScale;
            half3 _CloudOffset;
            half3 _DetailOffset;

            half _AbsorptionCoefficient;
            half _ScatteringAniso;

            float3 _BoundsMin;
            float3 _BoundsMax;

            #define PI 3.1415926

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                float3 viewDir = mul(unity_CameraInvProjection, float4(v.uv * 2 - 1, 0, -1));
                o.viewDir = mul(unity_CameraToWorld, float4(viewDir, 0));
                o.uv = v.uv;
                return o;
            }
            float sampleDensity(float3 rayPos){
                float3 uvw = rayPos * _CloudScale * 0.05 - _CloudOffset * 0.01;
                float shape = _CloudShapeNoise.SampleLevel(sampler_CloudShapeNoise, uvw, 0).r;

                uvw = rayPos * _DetailScale * 0.005 - _DetailOffset * 0.001;
                float detail = _CloudDetailNoise.SampleLevel(sampler_CloudDetailNoise, uvw, 0).r * 4;

                //TODO: Add in settings for tapering
                float taper = 1 - (rayPos.y - 1500) / 2000;
                taper = saturate(taper);

                float density = max(0, _DensityThreshold * taper - shape) * _DensityMultiplier - detail;
                return density;
            }
            float2 rayBoxDst(float3 boundsMin, float3 boundsMax, float3 rayOrigin, float3 rayDir){
                float3 t0 = (boundsMin - rayOrigin) / rayDir;
                float3 t1 = (boundsMax - rayOrigin) / rayDir;

                float3 tmin = min(t0, t1);
                float3 tmax = max(t0, t1);

                float dstA = max(max(tmin.x, tmin.y), tmin.z);
                float dstB = min(tmax.x, min(tmax.y, tmax.z));

                float dstToBox = max(0, dstA);
                return float2(dstToBox, max(0, dstB - dstToBox));
            }
            float henyeyGreenstein(float g, float mu) {
                float gg = g * g;
	            return (1.0 / (4.0 * PI))  * ((1.0 - gg) / pow(1.0 + gg - 2.0 * g * mu, 1.5));
            }
            float beersLaw(float dist, half absorption){
                return exp(-dist * absorption);
            }
            float lightMarch(float3 rayPos){
                float3 lightDir = _WorldSpaceLightPos0.xyz;

                float totalDensity = 0;
                float marchSize = 100;

                [unroll(2)]
                for (int i = 0; i < 2; i++){
                    rayPos += lightDir * marchSize;

                    totalDensity += max(0, sampleDensity(rayPos) * marchSize);
                }

                float transmittance = exp(-totalDensity * _AbsorptionCoefficient);
                return transmittance;
            }
            float LinearToDepth(float linearDepth)
            {
                return (1.0 - _ZBufferParams.w * linearDepth) / (linearDepth * _ZBufferParams.z);
            }
            fixed4 frag (v2f i, out float shadowDepth : SV_Depth) : SV_Target
            {
                //Sample screen color
                float4 screenColor = tex2D(_MainTex, i.uv);

                //Sample blue noise
                half2 normalizedScreenUV = i.uv;
                normalizedScreenUV.x *= (_ScreenParams.x / _ScreenParams.y);
                float noise = tex2D(_BlueNoise, normalizedScreenUV) * 4;
                noise = frac(noise + float(_Time.y % 8) / sqrt(0.5));

                //Create ray
                float3 rayOrigin = _WorldSpaceCameraPos;
                float viewLength = length(i.viewDir);
                float3 rayDirection = i.viewDir / viewLength;

                //Sample depth
                float nonLinearDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                float depth = LinearEyeDepth(nonLinearDepth) * viewLength;

                //Sample cloud bounds
                float2 rayBox = rayBoxDst(_BoundsMin, _BoundsMax, rayOrigin, rayDirection);

                //Create marching variables
                float stepSize = rayBox.y / 128;
                float dt = noise * stepSize;
                float dstLimit = min(depth - rayBox.x, rayBox.y);
                
                //Create light transport variables
                float phase = henyeyGreenstein(_ScatteringAniso, dot(rayDirection, _WorldSpaceLightPos0.xyz));
                float transmittance = 1;
                float lightEnergy = 0;

                //March
                while(dt < dstLimit){
                    float3 rayPos = rayOrigin + rayDirection * (rayBox.x + dt);
                    float density = sampleDensity(rayPos);

                    if (density > 0){
                        float lightTransmittance = lightMarch(rayPos);
                        lightEnergy += density * stepSize * transmittance * lightTransmittance * phase;
                        transmittance *= exp(-density * stepSize * _AbsorptionCoefficient);

                        if (transmittance < 0.01){
                            transmittance = 0;
                            break;
                        }
                        dt += stepSize;
                    }
                    else
                        dt += max(-density, stepSize);
                }
                float3 cloudColor = lightEnergy * _LightColor0.rgb;
                float3 col = screenColor * transmittance + cloudColor;
                return float4(col, 0);
            }
            ENDCG
        }
    }
}
