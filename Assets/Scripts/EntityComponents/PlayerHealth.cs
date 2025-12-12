using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private HealthComponent m_HealthComponent;

    private bool m_Shielded;

    private float m_DamageTimer;

    private void Awake()
    {
        m_DamageTimer = 0.5f;
    }
    private void Update()
    {
        if (!m_Shielded)
        {
            if (m_DamageTimer <= 0)
            {
                m_DamageTimer += 0.5f;
                m_HealthComponent.ChangeHealth(-1);
            }

            m_DamageTimer -= Time.deltaTime;
        }
    }
    public void SetShieldedState(bool state)
    {
        m_Shielded = state;
    }
}