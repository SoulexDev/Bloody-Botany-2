using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedShop : Interactable
{
    [SerializeField] private InventoryItem m_SeedItem;
    [SerializeField] private int m_ItemAmount;
    [SerializeField] private int m_Cost;

    public override void Awake()
    {
        base.Awake();

        m_ViewInfo.infoString = $"Purchase {m_SeedItem.name}s\n${m_Cost}";
    }
    public override void OnInteract()
    {
        base.OnInteract();

        if (Player.Instance.inventorySystem.CanStoreItem(m_SeedItem, m_ItemAmount) && Player.Instance.currencySystem.SpendCurrency(m_Cost))
        {
            Player.Instance.inventorySystem.AddItem(m_SeedItem, m_ItemAmount);
        }
    }
}