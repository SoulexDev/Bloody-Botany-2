using FishNet;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSpawner : NetworkBehaviour
{
    [SerializeField] private List<NetworkObject> m_Managers;
    private List<NetworkObject> m_SpawnedManagers = new List<NetworkObject>();

    public override void OnStartServer()
    {
        base.OnStartServer();
        m_Managers.ForEach(m =>
        {
            NetworkObject nob = Instantiate(m);
            InstanceFinder.ServerManager.Spawn(nob);

            m_SpawnedManagers.Add(nob);
        });
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsHostStarted)
            m_SpawnedManagers.ForEach(m => m.GiveOwnership(LocalConnection));
    }
}