using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_CurrencyText;

    private int m_CurrencyAmountBuffer = 100;
    private int m_CurrencyAmount
    {
        get { return m_CurrencyAmountBuffer; }
        set
        {
            if (m_CurrencyAmountBuffer > value)
            {
                
            }
            m_CurrencyAmountBuffer = value;
        }
    }
    private int m_CurrencyDisplayAmount;

    private void FixedUpdate()
    {
        if (m_CurrencyDisplayAmount < m_CurrencyAmount)
        {
            m_CurrencyDisplayAmount += 2;
        }
        if (m_CurrencyDisplayAmount > m_CurrencyAmount)
        {
            m_CurrencyDisplayAmount = m_CurrencyAmount;
        }

        m_CurrencyText.text = $"${m_CurrencyDisplayAmount}";
    }
    public bool SpendCurrency(int amount)
    {
        print(m_CurrencyAmount - amount);
        if (m_CurrencyAmount - amount < 0)
            return false;

        m_CurrencyAmount -= amount;
        return true;
    }
    public void AddCurrency(int amount)
    {
        m_CurrencyAmount += amount;
    }
}