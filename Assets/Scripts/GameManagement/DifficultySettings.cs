using UnityEngine;

[CreateAssetMenu(menuName = "Bloody Botany/Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    public float prepTimeSeconds = 90;
    public int maxGuns = 15;
    [Tooltip("Growth rate in seconds per stage")] public float plantGrowthRate = 1;
}