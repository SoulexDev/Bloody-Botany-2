using FishNet;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : NetworkBehaviour
{
    public static WaveManager Instance;

    [SerializeField] private NetworkObject m_SmogwalkerPrefab;
    public List<SpawnZone> spawnZones = new List<SpawnZone>();

    public int wave;
    private int m_MobCap;
    private int m_WaveEnemyCountTotal;
    private int m_CurrentEnemyCount;

    private void Awake()
    {
        Instance = this;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();

        StartCoroutine(HandleWaves());

        m_MobCap = GameManager.Instance.difficultySettings.mobCap * InstanceFinder.ClientManager.Clients.Count;
    }
    private IEnumerator HandleWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameManager.Instance.difficultySettings.prepTimeSeconds);

            m_WaveEnemyCountTotal = Mathf.RoundToInt(GameManager.Instance.difficultySettings.baseWaveEnemyCount + 
                GameManager.Instance.difficultySettings.addEnemyCountPerWaveMultiplier * wave);

            while (m_WaveEnemyCountTotal > 0)
            {
                if (m_CurrentEnemyCount + InstanceFinder.ClientManager.Clients.Count <= m_MobCap)
                    SpawnOnClients();
                yield return new WaitForSeconds(GameManager.Instance.difficultySettings.enemySpawnRate);
            }

            wave++;
        }
    }
    [ObserversRpc]
    public void SpawnOnClients()
    {
        if (spawnZones.Count == 0)
            return;

        SpawnZone zone = spawnZones[Random.Range(0, spawnZones.Count)];
        SpawnOnServer(zone.enemySpawns[Random.Range(0, zone.enemySpawns.Count)].position);
    }
    [ServerRpc(RequireOwnership = false)]
    public void SpawnOnServer(Vector3 position)
    {
        NetworkObject nob = InstanceFinder.NetworkManager.GetPooledInstantiated(m_SmogwalkerPrefab, 
            position, Quaternion.identity, true);

        InstanceFinder.ServerManager.Spawn(nob);

        m_CurrentEnemyCount++;
        m_WaveEnemyCountTotal--;
    }
    public void RemoveEnemy()
    {
        m_CurrentEnemyCount--;
    }
}