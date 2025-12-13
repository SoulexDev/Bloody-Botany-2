using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour, IHealth
{
    public int maxHealth = 15;

    private int m_HealthBuffer;
    public int health
    {
        get { return m_HealthBuffer; }
        set
        {
            //Lost health
            if (m_HealthBuffer > value)
            {
                OnHealthLost?.Invoke();
            }
            m_HealthBuffer = Mathf.Clamp(value, 0, maxHealth);

            if (m_HealthBuffer <= 0)
            {
                OnHealthDepleted?.Invoke();
            }

            OnHealthChanged?.Invoke();
        }
    }
    public delegate void HealthLost();
    public event HealthLost OnHealthLost;

    public delegate void HealthChanged();
    public event HealthChanged OnHealthChanged;

    public delegate void HealthDepleted();
    public event HealthDepleted OnHealthDepleted;

    private void Awake()
    {
        health = maxHealth;
    }
    public void ChangeHealth(int amount)
    {
        health += amount;
    }
}