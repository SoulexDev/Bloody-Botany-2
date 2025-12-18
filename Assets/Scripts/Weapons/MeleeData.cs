using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bloody Botany/Melee Data")]
public class MeleeData : ScriptableObject
{
    public int damage;
    public float hitRate = 0.75f;
    public int sweepCount = 2;
}