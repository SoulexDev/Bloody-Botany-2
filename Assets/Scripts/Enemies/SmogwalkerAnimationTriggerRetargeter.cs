using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmogwalkerAnimationTriggerRetargeter : MonoBehaviour
{
    [SerializeField] private Smogwalker m_Smogwalker;

    public void SetDamageTriggerActiveState(int state)
    {
        m_Smogwalker.SetDamageTriggerActiveState(state == 1);
    }
}