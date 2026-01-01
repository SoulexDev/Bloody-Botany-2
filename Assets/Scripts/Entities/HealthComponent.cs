using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IHealth
{
    public int maxHealth = 15;
    public bool callHealthLostEventOnDeplete = true;
    public bool clampHealth = true;

    private int m_HealthBuffer;
    public int health
    {
        get { return m_HealthBuffer; }
        set
        {
            float lastHealth = m_HealthBuffer;

            if (clampHealth)
                m_HealthBuffer = Mathf.Clamp(value, 0, maxHealth);
            else
                m_HealthBuffer = Mathf.Max(value, 0);

            if (m_HealthBuffer <= 0)
            {
                OnHealthDepleted?.Invoke();

                if (callHealthLostEventOnDeplete)
                    OnHealthLost?.Invoke();
            }
            else if (lastHealth > value)
            {
                OnHealthLost?.Invoke();
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
    public void ChangeHealth(int amount, ref bool died)
    {
        died = health + amount <= 0;

        health += amount;
    }
}