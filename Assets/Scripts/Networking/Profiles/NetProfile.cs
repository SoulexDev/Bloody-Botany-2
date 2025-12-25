using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Steamworks;
using UnityEngine;

public class NetProfile : NetworkBehaviour
{
    public static NetProfile Instance;

    [SerializeField] public readonly SyncVar<string> playerName = new SyncVar<string>();
    [SerializeField] public readonly SyncVar<SteamProfileImage> profileImage = new SyncVar<SteamProfileImage>();

    [SerializeField] private LobbyVisual m_LobbyVisualPrefab;
    private LobbyVisual m_LobbyVisualInstance;

    private bool m_PlayerNameReady;
    private bool m_ProfileImageReady;

    public override void OnStartClient()
    {
        base.OnStartClient();

        m_LobbyVisualInstance = Instantiate(m_LobbyVisualPrefab).Initialize(this);

        if (IsOwner)
        {
            Instance = this;
            if (SteamManager.Initialized)
            {
                SetPlayerName(SteamFriends.GetPersonaName());
                SetPlayerProfile(GetUserAvatar(SteamFriends.GetMediumFriendAvatar(SteamUser.GetSteamID())));
            }
            else
            {
                SetPlayerName(LocalConnection.ClientId.ToString());
            }

            AddProfile();
        }
    }
    public override void OnStopClient()
    {
        base.OnStopClient();

        if (m_LobbyVisualInstance)
            Destroy(m_LobbyVisualInstance.gameObject);

        if (IsOwner)
            RemoveProfile();
    }
    [ObserversRpc]
    public void SpawnLobbyVisual()
    {
        print("SEXXXX");
        m_LobbyVisualInstance = Instantiate(m_LobbyVisualPrefab).Initialize(this);

        m_LobbyVisualInstance.SetPFP(profileImage.Value);
    }
    [ServerRpc]
    private void AddProfile()
    {
        NetProfileManager.Instance.netProfiles.Add(this);
    }
    [ServerRpc]
    private void RemoveProfile()
    {
        NetProfileManager.Instance.netProfiles.Remove(this);
    }
    [ServerRpc]
    private void SetPlayerName(string name)
    {
        playerName.Value = name;
    }
    [ServerRpc]
    private void SetPlayerProfile(SteamProfileImage image)
    {
        profileImage.Value = image;
    }
    public static Texture2D GetTexture2DFromSteamProfileImage(SteamProfileImage image)
    {
        if (!image.IsValid())
            return null;

        Texture2D tex = new Texture2D(image.imageWidth, image.imageHeight, TextureFormat.RGBA32, false, false);
        tex.LoadRawTextureData(image.imgBytes);
        tex.Apply();
        return tex;
    }
    private SteamProfileImage GetUserAvatar(int imageID)
    {
        uint ImageWidth;
        uint ImageHeight;
        bool success = SteamUtils.GetImageSize(imageID, out ImageWidth, out ImageHeight);

        if (success && ImageWidth > 0 && ImageHeight > 0)
        {
            byte[] Image = new byte[ImageWidth * ImageHeight * 4];
            //Texture2D returnTexture = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
            success = SteamUtils.GetImageRGBA(imageID, Image, (int)(ImageWidth * ImageHeight * 4));
            if (success)
            {
                return new SteamProfileImage(Image, (int)ImageWidth, (int)ImageHeight);
            }
        }
        return new SteamProfileImage(null, 0, 0);
    }
    public struct SteamProfileImage
    {
        public byte[] imgBytes;
        public int imageWidth;
        public int imageHeight;

        public SteamProfileImage(byte[] bytes, int imgWidth, int imgHeight)
        {
            imgBytes = bytes;
            imageWidth = imgWidth;
            imageHeight = imgHeight;
        }
        public bool IsValid()
        {
            return !(imgBytes == null || imageWidth <= 0 || imageHeight <= 0);
        }
    }
}