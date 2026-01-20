#include "UnityCG.cginc"
sampler2D _CameraDepthTexture;

float GetDepth_float(float3 viewDir, float4 screenUV){
	float viewLength = length(viewDir);
	float nonLinearDepth = tex2D(_CameraDepthTexture, screenUV.xy / screenUV.w);
    return LinearEyeDepth(nonLinearDepth) * viewLength;
}