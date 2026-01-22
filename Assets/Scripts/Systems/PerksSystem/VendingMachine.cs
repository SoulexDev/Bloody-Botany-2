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

        m_ViewInfo.activeInfoString = $"Buy {m_GivenPerk.name}\n${m_Cost}";
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
            StatsManager.Instance.AddPerk(m_GivenPerk);
        }
    }
}