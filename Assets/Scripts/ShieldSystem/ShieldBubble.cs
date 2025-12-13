using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBubble : MonoBehaviour
{
    private PlayerHealth m_PlayerHealth;
    private void OnTriggerEnter(Collider other)
    {
        print(other);
        if (other.CompareTag("Player"))
        {
            if (other.transform.TryGetComponent(out m_PlayerHealth))
            {
                m_PlayerHealth.SetShieldedState(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!m_PlayerHealth)
            {
                Debug.LogError("Bug! Player health was not assigned to shield bubble despite the player exiting the range");
                return;
            }
            m_PlayerHealth.SetShieldedState(false);
            m_PlayerHealth = null;
        }
    }
    private void OnDisable()
    {
        if (m_PlayerHealth)
        {
            m_PlayerHealth.SetShieldedState(false);
        }
    }
}