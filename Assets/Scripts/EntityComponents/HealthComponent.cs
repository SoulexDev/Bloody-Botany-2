using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IHealth
{
    [SerializeField] private int m_MaxHealth = 15;
    private int m_HealthBuffer;
    private int m_Health
    {
        get { return m_HealthBuffer; }
        set
        {
            //Lost health
            if (m_HealthBuffer > value)
            {
                OnHealthLost?.Invoke();
            }
            m_HealthBuffer = value;

            if (m_HealthBuffer <= 0)
            {
                OnHealthDepleted?.Invoke();
            }
        }
    }
    public delegate void HealthLost();
    public event HealthLost OnHealthLost;

    public delegate void HealthDepleted();
    public event HealthDepleted OnHealthDepleted;

    private void Awake()
    {
        m_Health = m_MaxHealth;
    }
    public void ChangeHealth(int amount)
    {
        m_Health += amount;
    }
}