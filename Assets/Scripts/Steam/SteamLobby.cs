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

    //public GameObject hostButton;
    //public TextMeshProUGUI lobbyNameText;
    //public int maxClients = 8;

    private void Start()
    {
        if (!SteamManager.Initialized)
            return;

        if (Instance == null)
            Instance = this;

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        //lobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
        //lobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);

        //GetLobbiesList();
    }
    public void HostLobby(int maxClients)
    {
        Debug.Log("Attempting To Host Lobby...");
        //startButton.SetActive(true);
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, maxClients);

        //canvas.SetActive(false);
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
        //SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString() + "'s Lobby");

        GUIUtility.systemCopyBuffer = $"{callback.m_ulSteamIDLobby}";
    }
    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Request To Join Lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        currentLobbyID = callback.m_ulSteamIDLobby;

        if (InstanceFinder.IsServerStarted)
            return;

        //m_FishySteamworks.SetClientAddress(SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey));
        InstanceFinder.ClientManager.StartConnection(SteamMatchmaking.GetLobbyOwner(new CSteamID(currentLobbyID)).ToString());
    }
    public void JoinLobby(CSteamID lobbyID)
    {
        SteamMatchmaking.JoinLobby(lobbyID);
        //startButton.SetActive(false);
    }
    public void JoinFromLobbyID()
    {
        if (string.IsNullOrEmpty(GUIUtility.systemCopyBuffer))
            return;

        CSteamID joinID = new CSteamID(ulong.Parse(GUIUtility.systemCopyBuffer));
        SteamMatchmaking.JoinLobby(joinID);
    }
    //public void GetLobbiesList()
    //{
    //    if (lobbyIDs.Count > 0)
    //        lobbyIDs.Clear();
    //    SteamMatchmaking.AddRequestLobbyListResultCountFilter(60);
    //    SteamMatchmaking.RequestLobbyList();
    //}
    //void OnGetLobbyList(LobbyMatchList_t result)
    //{
    //    if (ServerList.instance.listOfLobbies.Count > 0)
    //        ServerList.instance.DestroyLobbies();
    //    for (int i = 0; i < result.m_nLobbiesMatching; i++)
    //    {
    //        CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
    //        lobbyIDs.Add(lobbyID);
    //        SteamMatchmaking.RequestLobbyData(lobbyID);
    //    }
    //}
    //void OnGetLobbyData(LobbyDataUpdate_t result)
    //{
    //    ServerList.instance.DisplayLobbies(lobbyIDs, result);
    //}
}