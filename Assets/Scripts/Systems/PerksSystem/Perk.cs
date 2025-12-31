using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PerkType { Speed_Healing, Damage_Firing, Accuracy_Magsize, LungCapacity }
[CreateAssetMenu(menuName = "Bloody Botany/Perk")]
public class Perk : ScriptableObject
{
    public PerkType perkType;
    public Sprite perkIcon;
    public MathFunction mathFunction;
    public float coefficient;
}