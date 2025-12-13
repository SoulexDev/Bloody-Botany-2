//==========================================( Neverway 2025 )=========================================================//
// Author
//  Liz M.
//
// Contributors
//
//
//====================================================================================================================//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundEnemySpawner : MonoBehaviour
{
    #region========================================( Variables )======================================================//
    /*-----[ Inspector Variables ]------------------------------------------------------------------------------------*/
    public float delay=5;


    /*-----[ External Variables ]-------------------------------------------------------------------------------------*/


    /*-----[ Internal Variables ]-------------------------------------------------------------------------------------*/
    private float spawnTimer = 0;
    private List<ZombieSpawner> activeZombieSpawns = new List<ZombieSpawner>();


    /*-----[ Reference Variables ]------------------------------------------------------------------------------------*/
    [SerializeField] private Smogwalker smogwalkerPrefab;


    #endregion


    #region=======================================( Functions )=======================================================//
    /*-----[ Mono Functions ]-----------------------------------------------------------------------------------------*/
    private void Update()
    {
        if (GameManager.Instance.gameStage != GameStage.Waves)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= delay)
        {
            spawnTimer -= delay;

            Transform spawn = GetRandomActiveSpawn();

            Instantiate(smogwalkerPrefab, spawn.position, spawn.rotation);
        }
    }


    /*-----[ Internal Functions ]-------------------------------------------------------------------------------------*/
    private Transform GetRandomActiveSpawn()
    {
        var allSpawners = FindObjectsOfType<ZombieSpawner>();
        foreach (var spawner in allSpawners)
        {
            if (spawner.isActive) activeZombieSpawns.Add(spawner);
        }

        return activeZombieSpawns[Random.Range(0, activeZombieSpawns.Count)].transform;
    }


    /*-----[ External Functions ]-------------------------------------------------------------------------------------*/


    #endregion
}
