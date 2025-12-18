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
    public int GetEnemyHealth(int baseNum, float buff)
    {
        return Mathf.RoundToInt(baseNum + difficultySettings.enemyHealthBuffMultiplicationRate * WaveManager.Instance.wave * buff);
    }
    public float GetEnemySpeed(float baseNum, float buff)
    {
        float mult = 1f + difficultySettings.enemySpeedBuffMultiplicationRate * WaveManager.Instance.wave * buff;
        return baseNum * mult;
    }
    public int GetEnemyDamage(int baseNum, float buff)
    {
        return Mathf.RoundToInt(baseNum + difficultySettings.enemyDamageBuffMultiplicationRate * WaveManager.Instance.wave * buff);
    }
}