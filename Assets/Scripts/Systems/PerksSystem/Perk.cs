using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum PerkType { MaxHealth_Healing, MoveSpeed_InteractionSpeed, Damage_FiringSpeed, Accuracy_Magsize, LungCapacity }
[CreateAssetMenu(menuName = "Bloody Botany/Perk")]
[ExecuteInEditMode]
public class Perk : ScriptableObject
{
    public PerkType perkType;
    public Sprite perkIcon;
    public MathFunction mathFunction;
    public float coefficient;
}
#if UNITY_EDITOR
[CustomEditor(typeof(Perk))]
public class PerkEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (Perk)target;

        GUI.Label(new Rect(20, 110, 100, 20),
            $"f(10) = {MathFunctions.GetFromFunction(1, script.coefficient, 10, script.mathFunction)}");

        GUI.Label(new Rect(20, 130, 100, 20),
            $"f(0) = {MathFunctions.GetFromFunction(1, script.coefficient, 0, script.mathFunction)}");
    }
}
#endif