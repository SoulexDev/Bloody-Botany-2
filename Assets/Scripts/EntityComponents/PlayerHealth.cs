using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private HealthComponent m_HealthComponent;
    [SerializeField] private TextMeshProUGUI m_OxygenText;
    [SerializeField] private TextMeshProUGUI m_StatusText;
    [SerializeField] private Image m_StatusImage;
    [SerializeField] private Image m_StatusBGImage;
    [SerializeField] private Sprite m_ShieldIcon;
    [SerializeField] private Sprite m_WarningIcon;
    [SerializeField] private Color m_ShieldedColor;
    [SerializeField] private Color m_UnShieldedColor;

    private bool m_ShieldedBuffer;
    private bool m_Shielded
    {
        get { return m_ShieldedBuffer; }
        set
        {
            m_ShieldedBuffer = value;

            m_StatusImage.sprite = m_ShieldedBuffer ? m_ShieldIcon : m_WarningIcon;
            m_StatusBGImage.color = m_ShieldedBuffer ? m_ShieldedColor : m_UnShieldedColor;
            m_StatusText.text = m_ShieldedBuffer ? "Protected" : "Unprotected";
        }
    }

    private float m_DamageTimer;
    private float m_HealTimer;

    private float m_OxygenValue;

    private void Awake()
    {
        m_DamageTimer = 0.5f;
        m_HealTimer = 1.5f;

        m_Shielded = false;
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
        else
        {
            if (m_HealTimer <= 0)
            {
                m_HealTimer += 1.5f;
                m_HealthComponent.ChangeHealth(1);
            }

            m_HealTimer -= Time.deltaTime;
        }

        m_OxygenValue = Mathf.MoveTowards(m_OxygenValue, m_Shielded ? 100 : 10, Time.deltaTime * 50);

        m_OxygenText.text = m_OxygenValue.ToString("f0");
    }
    public void SetShieldedState(bool state)
    {
        m_Shielded = state;
        print(m_Shielded);
    }
}