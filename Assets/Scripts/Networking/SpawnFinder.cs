using UnityEngine;

public class SpawnFinder : MonoBehaviour
{
    public static SpawnFinder Instance;

    public Transform[] m_Spawns;

    private void Awake()
    {
        Instance = this;
    }
}