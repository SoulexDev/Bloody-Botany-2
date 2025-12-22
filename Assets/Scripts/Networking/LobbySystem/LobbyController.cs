using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyController : NetworkBehaviour
{
    public static LobbyController Instance;

    private Scene m_ChosenScene;

    [SerializeField] private string m_SceneToLoad = "GameScene";
    [SerializeField] private List<Scene> m_Maps = new List<Scene>();
    [SerializeField] private NetworkObject m_GameProfile;

    private int m_LoadedClientsCount;
    private int m_SpawnIndex;

    private void Awake()
    {
        Instance = this;
        m_ChosenScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(m_SceneToLoad);

        InstanceFinder.SceneManager.OnClientPresenceChangeEnd += SceneManager_OnClientPresenceChangeEnd;
    }
    private void OnDestroy()
    {
        if (InstanceFinder.SceneManager)
            InstanceFinder.SceneManager.OnClientPresenceChangeEnd -= SceneManager_OnClientPresenceChangeEnd;
    }
    public void StartGame()
    {
        m_LoadedClientsCount = 0;

        SceneLoadData sld = new SceneLoadData(m_SceneToLoad);

        sld.ReplaceScenes = ReplaceOption.All;
        SceneManager.LoadGlobalScenes(sld);
        //sld.MovedNetworkObjects = NetProfileManager.Instance.netProfiles.Select(n=>n.GetComponent<NetworkObject>()).ToArray();

        //SceneManager.LoadConnectionScenes(sld);
    }
    [Server]
    private void SceneManager_OnClientPresenceChangeEnd(ClientPresenceChangeEventArgs obj)
    {
        if (obj.Scene.name != m_SceneToLoad)
            return;

        Debug.Log($"LOBBY CONTROLLER: {obj.Scene.name}");

        if (obj.Added)
        {
            m_LoadedClientsCount++;

            Debug.Log($"LOBBY CONTROLLER: {m_LoadedClientsCount}, " +
                $"SPAWN POSITION: {SpawnFinder.Instance.m_Spawns[m_SpawnIndex].position}, " +
                $"SPAWN INDEX: {m_SpawnIndex}," +
                $"SPAWN OBJECT: {SpawnFinder.Instance.m_Spawns[m_SpawnIndex]}, " +
                $"SPAWNS COUNT: {SpawnFinder.Instance.m_Spawns.Length}, " +
                $"SPAWNFINDER INSTANCE: {SpawnFinder.Instance}, " +
                $"SPAWNFINDER SPAWNS: {SpawnFinder.Instance.m_Spawns}, " +
                $"SPAWN NAME: {SpawnFinder.Instance.m_Spawns[m_SpawnIndex].name}");

            //NetworkObject nob = NetworkManager.GetPooledInstantiated(m_GameProfile,
            //            SpawnFinder.Instance.m_Spawns[m_SpawnIndex].position, Quaternion.identity, true);

            //InstanceFinder.ServerManager.Spawn(nob, NetProfileManager.Instance.netProfiles[m_SpawnIndex].Owner);

            //m_SpawnIndex++;
            //TODO: Refactor this if needed

            //print(m_LoadedClientsCount);
            //print(ClientManager.Clients.Count);
            //print(SpawnFinder.Instance);

            if (m_LoadedClientsCount == ClientManager.Clients.Count)
            {
                for (int i = 0; i < NetProfileManager.Instance.netProfiles.Count; i++)
                {
                    NetworkObject nob = NetworkManager.GetPooledInstantiated(m_GameProfile,
                        SpawnFinder.Instance.m_Spawns[i].position, Quaternion.identity, true);
                    InstanceFinder.ServerManager.Spawn(nob, NetProfileManager.Instance.netProfiles[i].Owner);
                    Debug.LogError(NetProfileManager.Instance.netProfiles[i].Owner);
                }
            }
        }
    }
}