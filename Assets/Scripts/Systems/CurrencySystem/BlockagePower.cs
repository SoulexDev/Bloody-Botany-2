using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockagePower : NetworkBehaviour
{
    private bool m_Bought = false;

    public void OnPowered()
    {
        TryDisableServer(LocalConnection);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void TryDisableServer(NetworkConnection conn)
    {
        if (m_Bought)
            return;
        else
            BoughtClientCallback(conn);

        m_Bought = true;
        DisableObservers();
    }
    
    [ObserversRpc]
    private void DisableObservers()
    {
        gameObject.SetActive(false);
    }
    
    [TargetRpc]
    private void BoughtClientCallback(NetworkConnection conn)
    {
    }
}