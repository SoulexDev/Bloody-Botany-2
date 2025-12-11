using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Smogwalker m_SmogwalkerPrefab;
    [SerializeField] private List<Transform> m_SpawnPositions;
    private float spawnTimer = 0;

    private void Update()
    {
        if (GameManager.Instance.gameStage != GameStage.Waves)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= 5)
        {
            spawnTimer -= 5;

            Transform spawn = m_SpawnPositions[Random.Range(0, m_SpawnPositions.Count)];

            Instantiate(m_SmogwalkerPrefab, spawn.position, spawn.rotation);
        }
    }
}