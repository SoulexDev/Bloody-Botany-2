using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockageShop : Interactable
{
    [SerializeField] private string m_BlockageName;
    [SerializeField] private int m_Cost;

    public override void Awake()
    {
        base.Awake();

        m_ViewInfo.infoString = $"Remove {m_BlockageName}\n${m_Cost}";
    }
    public override void OnInteract()
    {
        base.OnInteract();

        if (Player.Instance.currencySystem.SpendCurrency(m_Cost))
        {
            gameObject.SetActive(false);
        }
    }
}