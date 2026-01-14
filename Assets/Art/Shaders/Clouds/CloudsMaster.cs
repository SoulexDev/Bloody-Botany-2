using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class CloudsMaster : MonoBehaviour
{
    private enum TextureResolutions { K_16, K_32, K_64 }

    [SerializeField] private ComputeShader m_WorleyComputeShader;
    [SerializeField] private Material m_CloudsMaterial;
    [SerializeField] private TextureResolutions m_TextureResolution;

    [HideInInspector] public RenderTexture cloudsTex;
    [HideInInspector] public RenderTexture detailTex;

    [Header("Cloud Settings")]
    public float cloudScale = 1;
    public Vector3 cloudOffset;
    public float detailScale = 0.2f;
    public Vector3 detailOffset;

    [Range(0, 20)] public float densityMultiplier = 1;
    [Range(0, 0.05f)] public float densityThreshold = 0;

    [Header("Noise Settings")]
    [SerializeField][Range(1, 16)] private int m_CellsPerAxis;

    private List<ComputeBuffer> m_BuffersToRelease = new List<ComputeBuffer>();

    private void OnValidate()
    {
        Generate();
    }
    private void OnDestroy()
    {
        m_BuffersToRelease.ForEach(c => c.Release());
    }
    private bool CreateRenderTex(bool forced)
    {
        int textureRes = GetTextureRes();

        var format = RenderTextureFormat.RFloat;
        if (forced || (cloudsTex != null || detailTex != null) && (cloudsTex.width != textureRes || detailTex.width != textureRes))
        {
            ClearRenderTex();

            cloudsTex = new RenderTexture(textureRes, textureRes, 0, format);
            cloudsTex.volumeDepth = textureRes;
            cloudsTex.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
            cloudsTex.enableRandomWrite = true;
            cloudsTex.wrapMode = TextureWrapMode.Repeat;
            cloudsTex.filterMode = FilterMode.Bilinear;

            detailTex = new RenderTexture(textureRes, textureRes, 0, format);
            detailTex.volumeDepth = textureRes;
            detailTex.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
            detailTex.enableRandomWrite = true;
            detailTex.wrapMode = TextureWrapMode.Repeat;
            detailTex.filterMode = FilterMode.Bilinear;
            return true;
        }
        else if (cloudsTex == null || detailTex == null)
        {
            cloudsTex = new RenderTexture(textureRes, textureRes, 0, format);
            cloudsTex.volumeDepth = textureRes;
            cloudsTex.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
            cloudsTex.enableRandomWrite = true;
            cloudsTex.wrapMode = TextureWrapMode.Repeat;
            cloudsTex.filterMode = FilterMode.Bilinear;

            detailTex = new RenderTexture(textureRes, textureRes, 0, format);
            detailTex.volumeDepth = textureRes;
            detailTex.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
            detailTex.enableRandomWrite = true;
            detailTex.wrapMode = TextureWrapMode.Repeat;
            detailTex.filterMode = FilterMode.Bilinear;
            return true;
        }
        return false;
    }
    private void ClearRenderTex()
    {
        if (cloudsTex)
        {
            cloudsTex.Release();
            DestroyImmediate(cloudsTex);
        }
        if (detailTex)
        {
            detailTex.Release();
            DestroyImmediate(detailTex);
        }
    }
    private int GetTextureRes()
    {
        return m_TextureResolution switch
        {
            TextureResolutions.K_16 => 16,
            TextureResolutions.K_32 => 32,
            TextureResolutions.K_64 => 64,
            _ => 32
        };
    }
    public void Generate(bool forced = false)
    {
        m_BuffersToRelease.ForEach(c => c.Release());

        if (!CreateRenderTex(forced))
            return;

        //Shape texture
        if (cloudsTex != null)
        {
            m_WorleyComputeShader.SetTexture(0, "Result", cloudsTex);
            m_WorleyComputeShader.SetInt("Resolution", GetTextureRes());
            m_WorleyComputeShader.SetInt("CellsPerAxis", m_CellsPerAxis);
            CreateWorleyPoints("WorleyPoints", m_CellsPerAxis);
            m_WorleyComputeShader.Dispatch(0, 8, 8, 8);
        }

        //Detail texture
        if (detailTex != null)
        {
            m_WorleyComputeShader.SetTexture(0, "Result", detailTex);
            m_WorleyComputeShader.SetInt("Resolution", GetTextureRes());
            m_WorleyComputeShader.SetInt("CellsPerAxis", m_CellsPerAxis * 2);
            CreateWorleyPoints("WorleyPoints", m_CellsPerAxis * 2);
            m_WorleyComputeShader.Dispatch(0, 8, 8, 8);
        }
    }
    private void CreateWorleyPoints(string bufferName, int cellsPerAxis)
    {
        Vector3[] points = new Vector3[cellsPerAxis * cellsPerAxis * cellsPerAxis];
        Vector3 rand;

        float cellRecip = 1f / cellsPerAxis;

        for (int x = 0; x < cellsPerAxis; x++)
        {
            for (int y = 0; y < cellsPerAxis; y++)
            {
                for (int z = 0; z < cellsPerAxis; z++)
                {
                    rand.x = Random.value;
                    rand.y = Random.value;
                    rand.z = Random.value;

                    points[x + cellsPerAxis * (y + z * cellsPerAxis)] = (new Vector3(x, y, z) + rand) * cellRecip;
                }
            }
        }

        CreateBuffer(points, sizeof(float) * 3, bufferName);
    }
    private void CreateBuffer(System.Array data, int stride, string bufferName)
    {
        ComputeBuffer buffer = new ComputeBuffer(data.Length, stride, ComputeBufferType.Raw);
        m_BuffersToRelease.Add(buffer);
        buffer.SetData(data);
        m_WorleyComputeShader.SetBuffer(0, bufferName, buffer);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(CloudsMaster))]
public class CloudsMasterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (CloudsMaster)target;

        //EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("Force Generate"))
        {
            script.Generate(true);
        }

        //EditorGUI.EndChangeCheck();
        DrawDefaultInspector();

        GUILayout.Label(script.cloudsTex);
    }
}
#endif