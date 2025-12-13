using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class LobbyVisualSpawner : MonoBehaviour
{
    public static LobbyVisualSpawner Instance;

    public Transform lobbyVisualContainer;
    //[SerializeField] private NetworkObject m_LobbyVisualPrefab;
    private void Awake()
    {
        Instance = this;
    }
}