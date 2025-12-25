using FishNet.Object;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyVisual : MonoBehaviour
{
    private NetProfile m_NetProfile;

    [SerializeField] private RawImage m_ProfileImage;
    //[SerializeField] private TextMeshProUGUI m_NameText;

    public LobbyVisual Initialize(NetProfile profile = default)
    {
        m_NetProfile = profile;

        if (m_NetProfile)
        {
            m_NetProfile.playerName.OnChange += PlayerName_OnChange;
            m_NetProfile.profileImage.OnChange += ProfileImage_OnChange;
        }

        transform.SetParent(LobbyVisualSpawner.Instance.lobbyVisualContainer);
        transform.localScale = Vector3.one;

        return this;
    }
    private void OnDestroy()
    {
        if (m_NetProfile)
        {
            m_NetProfile.playerName.OnChange -= PlayerName_OnChange;
            m_NetProfile.profileImage.OnChange -= ProfileImage_OnChange;
        }
    }
    private void PlayerName_OnChange(string prev, string next, bool asServer)
    {
        //m_NameText.text = next;
    }
    private void ProfileImage_OnChange(NetProfile.SteamProfileImage prev, NetProfile.SteamProfileImage next, bool asServer)
    {
        m_ProfileImage.texture = NetProfile.GetTexture2DFromSteamProfileImage(next);
    }
    public void SetPFP(NetProfile.SteamProfileImage img)
    {
        m_ProfileImage.texture = NetProfile.GetTexture2DFromSteamProfileImage(img);
    }
}