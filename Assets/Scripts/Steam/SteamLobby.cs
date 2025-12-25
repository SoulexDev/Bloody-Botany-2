using UnityEngine;
using FishNet;
using Steamworks;
using FishySteamworks;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby Instance;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> joinRequest;
    protected Callback<LobbyEnter_t> lobbyEntered;

    //protected Callback<LobbyMatchList_t> lobbyList;
    //protected Callback<LobbyDataUpdate_t> lobbyDataUpdated;

    //public List<CSteamID> lobbyIDs = new List<CSteamID>();

    public ulong currentLobbyID;
    private const string HostAddressKey = "HostAddress";

    private CSteamID m_LobbyCSteamID;

    //public GameObject hostButton;
    //public TextMeshProUGUI lobbyNameText;
    //public int maxClients = 8;

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Init();
        }
    }
    public void Init()
    {
        Debug.LogWarning("INITIALIZING");
        if (!SteamManager.Initialized)
            return;

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        HostLobby(4);
    }
    public void HostLobby(int maxClients)
    {
        Debug.Log("Attempting To Host Lobby...");
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, maxClients);
    }
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        Debug.Log("Attempting To Create Lobby...");
        print(callback.m_eResult);
        if (callback.m_eResult != EResult.k_EResultOK)
            return;

        Debug.Log("Lobby Created Succesfully");
        InstanceFinder.ServerManager.StartConnection();
        InstanceFinder.ClientManager.StartConnection();

        m_LobbyCSteamID = new CSteamID(callback.m_ulSteamIDLobby);

        SteamMatchmaking.SetLobbyData(m_LobbyCSteamID, "name", SteamFriends.GetPersonaName().ToString() + "'s Lobby");
    }
    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Request To Join Lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        currentLobbyID = callback.m_ulSteamIDLobby;
        m_LobbyCSteamID = new CSteamID(callback.m_ulSteamIDLobby);

        if (InstanceFinder.IsServerStarted)
            return;

        InstanceFinder.ClientManager.StartConnection(SteamMatchmaking.GetLobbyOwner(new CSteamID(currentLobbyID)).ToString());
    }
    public void JoinLobby(CSteamID lobbyID)
    {
        SteamMatchmaking.JoinLobby(lobbyID);
    }
    public void JoinFromLobbyID()
    {
        if (string.IsNullOrEmpty(GUIUtility.systemCopyBuffer))
            return;

        CSteamID joinID = new CSteamID(ulong.Parse(GUIUtility.systemCopyBuffer));
        SteamMatchmaking.JoinLobby(joinID);
    }
    public void AddFriend()
    {
        GUIUtility.systemCopyBuffer = $"{currentLobbyID}";

        SteamFriends.ActivateGameOverlayInviteDialog(m_LobbyCSteamID);
    }
}