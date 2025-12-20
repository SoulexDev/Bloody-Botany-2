using UnityEngine;

[CreateAssetMenu(menuName = "Bloody Botany/Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    //TODO: Make enemy stuff use new math things.. like allowing us to pick the math func and coefficients
    [Tooltip("Seconds of downtime between each wave")] public float prepTimeSeconds = 90;
    [Tooltip("Growth rate in seconds per stage")] public float plantGrowthRate = 1;
    [Tooltip("Starting amount of enemies at wave 1, per player")] public int baseWaveEnemyCount = 2;
    [Tooltip("Additional enemy count multiplier per wave")] public float addEnemyCountPerWaveMultiplier = 1f;
    [Tooltip("Maximum amount of enemies per player at any given time")] public int mobCap = 16;
    [Tooltip("Seconds between enemy spawns")] public float enemySpawnRate = 4f;
    [Tooltip("Enemy health growth multipler per wave")] public float enemySpeedBuffMultiplicationRate = 1.25f;
    [Tooltip("Enemy speed growth multipler per wave")] public float enemyHealthBuffMultiplicationRate = 1.05f;
    [Tooltip("Enemy damage growth multipler per wave")] public float enemyDamageBuffMultiplicationRate = 1.025f;
    public int onKillPaymentLowerBound = 25;
    public int onKillPaymentUpperBound = 100;
    [Tooltip("Seconds between perk loss while downed")] public float perkLossRate = 1.5f;
}