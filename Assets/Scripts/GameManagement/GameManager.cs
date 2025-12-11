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

    private float stageTimer = 0;

    private void Awake()
    {
        Instance = this;

        playerIgnoreMask = ~LayerMask.GetMask("Player", "Ignore Player", "Ignore Raycast");
        enemyIgnoreMask = ~LayerMask.GetMask("Enemy", "Ignore Enemy", "Ignore Raycast");

        stageTimer = difficultySettings.prepTimeSeconds;
    }
    private void Update()
    {
        stageTimer -= Time.deltaTime;

        if (stageTimer <= 0)
        {
            stageTimer = 0;

            gameStage = GameStage.Waves;
        }
    }
}