using FishNet;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetProfileSpawner : MonoBehaviour
{
    [SerializeField] private NetworkObject m_NetProfile;
    private void Awake()
    {
        InstanceFinder.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
    }
    private void OnDestroy()
    {
        InstanceFinder.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedStartScenes;
    }
    private void SceneManager_OnClientLoadedStartScenes(FishNet.Connection.NetworkConnection conn, bool asServer)
    {
        if (!asServer)
            return;
        if (NetProfileManager.Instance.GetNetProfileByConnection(conn) != null)
            return;

        NetworkObject nob = InstanceFinder.NetworkManager.GetPooledInstantiated(m_NetProfile, asServer);
        InstanceFinder.ServerManager.Spawn(nob, conn);
    }
}