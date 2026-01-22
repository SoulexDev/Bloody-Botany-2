using UnityEngine;

[ExecuteAlways, ImageEffectAllowedInSceneView]
public class CloudRenderer : MonoBehaviour
{
    //[SerializeField] private Camera m_RenderCam;
    [SerializeField] private CloudsMaster m_CloudsMaster;
    [SerializeField] private Material m_CloudsMaterial;
    [SerializeField] private Transform m_CloudsBounds;

    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!m_CloudsMaterial)
            return;

        Camera.current.depthTextureMode = DepthTextureMode.Depth;

        m_CloudsMaterial.SetTexture("_MainTex", source);
        m_CloudsMaterial.SetTexture("_CloudShapeNoise", m_CloudsMaster.cloudsTex);
        m_CloudsMaterial.SetTexture("_CloudDetailNoise", m_CloudsMaster.detailTex);

        m_CloudsMaterial.SetFloat("_DensityMultiplier", m_CloudsMaster.densityMultiplier);
        m_CloudsMaterial.SetFloat("_DensityThreshold", m_CloudsMaster.densityThreshold);
        m_CloudsMaterial.SetFloat("_CloudScale", m_CloudsMaster.cloudScale);
        m_CloudsMaterial.SetVector("_CloudOffset", m_CloudsMaster.cloudOffset);
        m_CloudsMaterial.SetFloat("_DetailScale", m_CloudsMaster.detailScale);
        m_CloudsMaterial.SetVector("_DetailOffset", m_CloudsMaster.detailOffset);

        if (m_CloudsBounds)
        {
            m_CloudsMaterial.SetVector("_BoundsMin", m_CloudsBounds.position - m_CloudsBounds.localScale * 0.5f);
            m_CloudsMaterial.SetVector("_BoundsMax", m_CloudsBounds.position + m_CloudsBounds.localScale * 0.5f);
        }

        Graphics.Blit(source, destination, m_CloudsMaterial);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(m_CloudsBounds.position, m_CloudsBounds.localScale);
    }
}