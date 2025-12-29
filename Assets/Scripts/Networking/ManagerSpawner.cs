using FishNet;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSpawner : MonoBehaviour
{
    [SerializeField] private List<NetworkObject> m_Managers;
    private List<NetworkObject> m_SpawnedManagers = new List<NetworkObject>();

    private void Awake()
    {
        InstanceFinder.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
    }
    private void OnDestroy()
    {
        InstanceFinder.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;
    }
    private void ServerManager_OnServerConnectionState(FishNet.Transporting.ServerConnectionStateArgs obj)
    {
        switch (obj.ConnectionState)
        {
            case FishNet.Transporting.LocalConnectionState.Stopped:
                break;
            case FishNet.Transporting.LocalConnectionState.Stopping:
                break;
            case FishNet.Transporting.LocalConnectionState.Starting:
                break;
            case FishNet.Transporting.LocalConnectionState.Started:
                m_Managers.ForEach(m =>
                {
                    NetworkObject nob = Instantiate(m);
                    InstanceFinder.ServerManager.Spawn(nob);

                    m_SpawnedManagers.Add(nob);
                });
                break;
            default:
                break;
        }
    }
}