using FishNet;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyFeedbackController : MonoBehaviour
{
    [SerializeField] private GameObject m_SteamNotInitPanel;
    [SerializeField] private GameObject m_LobbyPanel;
    [SerializeField] private List<Button> m_LobbyButtons;

    private void Awake()
    {
        InstanceFinder.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
        m_SteamNotInitPanel.SetActive(!SteamAPI.IsSteamRunning());
    }
    private void OnDestroy()
    {
        if (InstanceFinder.ClientManager)
            InstanceFinder.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
    }
    public void RetrySteamworks()
    {
        if (SteamAPI.Init())
        {
            m_SteamNotInitPanel.SetActive(false);

            SteamLobby.Instance.Init();
        }
    }
    private void ClientManager_OnClientConnectionState(FishNet.Transporting.ClientConnectionStateArgs obj)
    {
        //TODO: DO something here soon
        return;
        switch (obj.ConnectionState)
        {
            case FishNet.Transporting.LocalConnectionState.Stopped:
                DebugConsole.Instance.PrintToConsole("Local connection stoppped.");
                DebugConsole.Instance.PrintWarningToConsole("If this is unexpected, please ensure that Steam is open, and that you are connected to the internet.");
                //m_LobbyPanel.SetActive(true);
                m_LobbyButtons.ForEach(b=>b.interactable = true);
                break;
            case FishNet.Transporting.LocalConnectionState.Stopping:
                DebugConsole.Instance.PrintToConsole("Stopping local connection...");
                break;
            case FishNet.Transporting.LocalConnectionState.Starting:
                DebugConsole.Instance.PrintToConsole("Starting local connection...");
                m_LobbyButtons.ForEach(b => b.interactable = false);
                break;
            case FishNet.Transporting.LocalConnectionState.Started:
                DebugConsole.Instance.PrintToConsole("Local connection started.");
                //m_LobbyPanel.SetActive(false);
                break;
            default:
                break;
        }
    }
}