using UnityEngine;

[CreateAssetMenu(menuName = "Bloody Botany/Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    [Tooltip("Seconds of downtime between each wave")] public float prepTimeSeconds = 90;
    [Tooltip("Growth rate in seconds per stage")] public float plantGrowthRate = 1;
    [Tooltip("Enemy health growth multipler per wave")] public float enemyWaveHealthMultiplicationRate = 1.25f;
    [Tooltip("Starting amount of enemies at wave 1, per player")] public int baseWaveEnemyCount = 2;
    [Tooltip("Additional enemy count multiplier per wave")] public float addEnemyCountPerWaveMultiplier = 1f;
    [Tooltip("Maximum amount of enemies per player at any given time")] public int mobCap = 16;
    [Tooltip("Seconds between enemy spawns")] public float enemySpawnRate = 4f;
}