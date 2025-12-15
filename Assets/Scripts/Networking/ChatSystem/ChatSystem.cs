using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Transporting;
using Steamworks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class ChatSystem : MonoBehaviour
{
    //[SerializeField] private List<EventReference> m_ChatSounds;

    [SerializeField] private TextMeshProUGUI m_ChatText;
    [SerializeField] private TMP_InputField m_InputField;

    private string m_Username;

    private void Awake()
    {
        m_InputField.onSelect.AddListener((s) => FocusChatbox());
    }
    private void OnDestroy()
    {
        m_InputField.onSelect.RemoveAllListeners();
    }
    private void Start()
    {
        if (SteamManager.Initialized)
            m_Username = SteamFriends.GetPersonaName();
        else
            m_Username = InstanceFinder.ClientManager.Connection.ClientId.ToString();
    }
    private void OnEnable()
    {
        InstanceFinder.ClientManager.RegisterBroadcast<ChatBroadcast>(OnChatBroadcastClient);
        InstanceFinder.ServerManager.RegisterBroadcast<ChatBroadcast>(OnChatBroadcastServer);
    }
    private void OnDisable()
    {
        if (InstanceFinder.ClientManager)
            InstanceFinder.ClientManager.UnregisterBroadcast<ChatBroadcast>(OnChatBroadcastClient);
        if (InstanceFinder.ServerManager)
            InstanceFinder.ServerManager.UnregisterBroadcast<ChatBroadcast>(OnChatBroadcastServer);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChatBroadcast chat = new ChatBroadcast(m_Username, m_InputField.text, Color.white);
            m_InputField.text = "";
            m_InputField.ActivateInputField();
            m_InputField.Select();
            InstanceFinder.ClientManager.Broadcast(chat);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            UnfocusChatbox();
    }
    private void FocusChatbox()
    {
        //GameProfile.Instance.canControl = false;
    }
    private void UnfocusChatbox()
    {
        //GameProfile.Instance.canControl = true;
    }
    private void OnChatBroadcastServer(NetworkConnection conn, ChatBroadcast chat, Channel channel)
    {
        if (conn.FirstObject == null)
            return;

        if (conn != InstanceFinder.ClientManager.Connection)
            chat.soundEventID = -1;

        chat.message = Regex.Replace(chat.message, "\r\n", string.Empty);

        if (string.IsNullOrEmpty(chat.message) || string.IsNullOrWhiteSpace(chat.message))
            return;

        string hexColor = ColorUtility.ToHtmlStringRGBA(chat.usernameColor);
        chat.message = $"<color=#{hexColor}>[{chat.username}]</color> {chat.message}";

        InstanceFinder.ServerManager.Broadcast(chat, true, channel);
    }
    private void OnChatBroadcastClient(ChatBroadcast chat, Channel channel)
    {
        m_ChatText.text += $"{chat.message}\n";

        //if (chat.soundEventID >= 0)
        //{
        //    Audio_FMODAudioManager.PlayOneShot(m_ChatSounds[chat.soundEventID]);
        //}
    }
}
public struct ChatBroadcast : IBroadcast
{
    public string username;
    public string message;
    public Color usernameColor;
    public int soundEventID;

    public ChatBroadcast(string username, string message, Color usernameColor)
    {
        this.username = username;
        this.message = message;
        this.usernameColor = usernameColor;
        this.soundEventID = -1;
    }
}