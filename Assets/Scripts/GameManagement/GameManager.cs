using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStage { Prep, Waves }
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameStage gameStage;
    public DifficultySettings difficultySettings;

    public LayerMask playerIgnoreMask;
    public LayerMask enemyIgnoreMask;

    public LayerMask seedThrowMask;

    private void Awake()
    {
        Instance = this;

        playerIgnoreMask = ~LayerMask.GetMask("Player", "Ignore Player", "Ignore Raycast");
        enemyIgnoreMask = ~LayerMask.GetMask("Enemy", "Ignore Enemy", "Ignore Raycast");
        seedThrowMask = ~LayerMask.GetMask("Player", "Enemy", "Ignore Player", "Ignore Enemy", "Ignore Raycast");
    }
    public float GetEnemyHealthMultiplier()
    {
        return 1f + difficultySettings.enemyWaveHealthMultiplicationRate * WaveManager.Instance.wave;
    }
}