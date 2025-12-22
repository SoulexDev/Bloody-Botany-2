using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using TMPro;
using UnityEngine;

public class CurrencySystem : NetworkBehaviour
{
    [SerializeField] private AudioClip m_MoneyAcquiredSound;
    [SerializeField] private AudioSource m_Source;
    private TextMeshProUGUI m_CurrencyText => CanvasFinder.Instance.currencyText;

    private int m_CurrencyAmountBuffer = 250;
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
        if (!IsOwner)
            return;

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
        //print(m_CurrencyAmount - amount);
        if (m_CurrencyAmount - amount < 0)
            return false;

        m_CurrencyAmount -= amount;
        return true;
    }
    public bool HasCurrency(int amount)
    {
        return m_CurrencyAmount - amount >= 0;
    }
    public void AddCurrency(int amount)
    {
        m_Source.PlayOneShot(m_MoneyAcquiredSound);
        m_CurrencyAmount += amount;
    }
}