using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour, IHealth
{
    [SerializeField] private int m_MaxHealth = 15;
    [SerializeField] private Image m_HealthBar;
    [SerializeField] private TextMeshProUGUI m_HealthText;

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
            m_HealthBuffer = Mathf.Clamp(value, 0, m_MaxHealth);

            if (m_HealthBar)
                m_HealthBar.fillAmount = (float)m_HealthBuffer / m_MaxHealth;
            if (m_HealthText)
                m_HealthText.text = m_HealthBuffer.ToString("D2") + "/" + m_MaxHealth.ToString("D2");

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