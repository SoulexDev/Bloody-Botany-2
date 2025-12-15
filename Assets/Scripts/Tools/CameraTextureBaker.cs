using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraTextureBaker : MonoBehaviour
{
    [SerializeField] private Camera m_BakeCamera;
    [SerializeField] private Terrain m_BakeTerrain;
    [SerializeField] private RenderTexture m_BakeTexture;

    [ContextMenu("Bake")]
    public void Bake()
    {
        transform.position = m_BakeTerrain.transform.position + 
            new Vector3(
                m_BakeTerrain.terrainData.size.x * 0.5f, 
                m_BakeTerrain.terrainData.size.y, 
                m_BakeTerrain.terrainData.size.z * 0.5f);

        m_BakeCamera.orthographicSize = Mathf.Max(m_BakeTerrain.terrainData.size.x, m_BakeTerrain.terrainData.size.z) * 0.5f;

        Shader.SetGlobalFloat("_OrthoScale", m_BakeCamera.orthographicSize * 2);
        Shader.SetGlobalFloat("_OffsetX", transform.position.x - m_BakeCamera.orthographicSize);
        Shader.SetGlobalFloat("_OffsetY", transform.position.y - m_BakeCamera.farClipPlane);
        Shader.SetGlobalFloat("_OffsetZ", transform.position.z - m_BakeCamera.orthographicSize);
        Shader.SetGlobalFloat("_FarClip", m_BakeCamera.farClipPlane);

        m_BakeCamera.enabled = true;
        m_BakeCamera.targetTexture = m_BakeTexture;
        Shader.SetGlobalTexture("_TerrainTex", m_BakeTexture);
        m_BakeCamera.Render();
        m_BakeCamera.enabled = false;
        m_BakeCamera.targetTexture = null;
    }
}