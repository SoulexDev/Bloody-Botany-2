using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockageShop : Interactable
{
    [SerializeField] private string m_BlockageName;
    [SerializeField] private int m_Cost;

    private bool m_Bought = false;

    public override void Awake()
    {
        base.Awake();

        m_ViewInfo.infoString = $"Remove {m_BlockageName}\n${m_Cost}";
    }
    //TODO: CLIENT CALLBACKS CLIENT CALLBACKS CLIENT CALLBACKSSSSS!!
    public override void OnInteract()
    {
        base.OnInteract();

        if (GameProfile.Instance.currencySystem.HasCurrency(m_Cost))
        {
            TryDisableServer(LocalConnection);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void TryDisableServer(NetworkConnection conn, Channel channel = Channel.Unreliable)
    {
        if (m_Bought)
            return;
        else
            BoughtClientCallback(conn);

        m_Bought = true;
        DisableObservers();
    }
    [ObserversRpc]
    private void DisableObservers(Channel channel = Channel.Unreliable)
    {
        gameObject.SetActive(false);
    }
    [TargetRpc]
    private void BoughtClientCallback(NetworkConnection conn, Channel channel = Channel.Unreliable)
    {
        if (!GameProfile.Instance.currencySystem.SpendCurrency(m_Cost))
            Debug.LogError("Player somehow doesn't have enough currency, even with the client checking");
    }
}