using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : Interactable
{
    [SerializeField] private Perk m_GivenPerk;
    [SerializeField] private int m_Cost = 250;

    public override void Awake()
    {
        base.Awake();

        if (m_PowerSource && !m_PowerSource.activated.Value)
            m_ViewInfo.infoString = "Requires Power";
        else
            m_ViewInfo.infoString = $"Buy {m_GivenPerk.name}\n${m_Cost}";
    }
    public override void OnInteract()
    {
        base.OnInteract();

        //for (int i = 0; i < 20; i++)
        //{
        //    PerksManager.Instance.AddPerk(m_GivenPerk);
        //}
        if (GameProfile.Instance.currencySystem.SpendCurrency(m_Cost))
        {
            PerksManager.Instance.AddPerk(m_GivenPerk);
        }
    }
}