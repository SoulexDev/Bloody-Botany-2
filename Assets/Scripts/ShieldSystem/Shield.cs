using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShieldState { Inactive, Active, Broken, Locked }
public class Shield : Interactable, IHealth
{
    private ShieldState m_ShieldStateBuffer;
    public ShieldState shieldState
    {
        get { return m_ShieldStateBuffer; }
        set
        {
            if (m_ShieldStateBuffer != value)
            {
                m_ShieldStateBuffer = value;

                UpdateInteractionText();

                OnShieldStateChanged?.Invoke();
                m_ShieldVolume.SetActive(m_ShieldStateBuffer == ShieldState.Active);
            }
        }
    }
    public delegate void ShieldStateChanged();
    public event ShieldStateChanged OnShieldStateChanged;

    [SerializeField] private int m_MaxHealth = 100;
    [SerializeField] private int m_RepairCost;
    [SerializeField] private GameObject m_ShieldVolume;
    private int m_Health;

    public override void Awake()
    {
        base.Awake();

        shieldState = ShieldState.Inactive;

        UpdateInteractionText();
    }
    public void ChangeHealth(int amount)
    {
        if (shieldState == ShieldState.Locked || shieldState == ShieldState.Broken)
            return;

        m_Health += amount;

        print(m_Health);

        if (m_Health <= 0)
        {
            shieldState = ShieldState.Broken;
        }
    }
    public override void OnInteract()
    {
        base.OnInteract();

        switch (shieldState)
        {
            case ShieldState.Inactive:
                if (ShieldManager.Instance.TrySetCurrentShield(this))
                    shieldState = ShieldState.Active;
                else
                    print("Cant set shield");
                break;
            case ShieldState.Active:
                shieldState = ShieldState.Locked;
                break;
            case ShieldState.Broken:
                if (Player.Instance.currencySystem.SpendCurrency(m_RepairCost))
                {
                    if (ShieldManager.Instance.TrySetCurrentShield(this))
                        shieldState = ShieldState.Active;
                    else
                        shieldState = ShieldState.Inactive;

                    m_Health = m_MaxHealth;
                }
                break;
            case ShieldState.Locked:
                if (ShieldManager.Instance.TrySetCurrentShield(this))
                    shieldState = ShieldState.Active;
                else
                    print("Cant set shield");
                break;
            default:
                break;
        }
    }
    private void UpdateInteractionText()
    {
        m_ViewInfo.infoString = shieldState switch
        {
            ShieldState.Inactive => "Activate Shield",
            ShieldState.Active => "Lockdown Shield",
            ShieldState.Broken => $"Repair Shield\n${m_RepairCost}",
            ShieldState.Locked => "Unlock Shield",
            _ => ""
        };
    }
}