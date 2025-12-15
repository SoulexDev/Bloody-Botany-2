using FishNet.Object;
using FishNet.Object.Synchronizing;
using Steamworks;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameProfile : NetworkBehaviour
{
    public static GameProfile Instance;

    public PlayerController playerController;
    public InventorySystem inventorySystem;
    public CurrencySystem currencySystem;
    public PlayerHealth playerHealth;

    public GameObject visual;

    public readonly SyncVar<string> steamName = new SyncVar<string>();
    [SerializeField] private TextMeshPro m_NameTag;

    private void Awake()
    {
        steamName.OnChange += OnChangeName;
    }
    private void OnDestroy()
    {
        steamName.OnChange -= OnChangeName;
    }
    private void OnChangeName(string prev, string next, bool asServer)
    {
        m_NameTag.text = next;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            Instance = this;
            visual.SetActive(false);
            //car.tag = "Player";

            AddProfile();

            if (SteamManager.Initialized)
                ChangeName(SteamFriends.GetPersonaName());
        }
    }
    public override void OnStopClient()
    {
        base.OnStopClient();

        if (IsOwner)
            RemoveProfile();
    }
    public void AssignTeam(int team)
    {
        StartCoroutine(WaitForClient(team));
    }
    IEnumerator WaitForClient(int team)
    {
        while (!IsClientStarted)
            yield return null;

        //Material[] mats = carRenderer.sharedMaterials;

        //if (team != GameProfileManager.Instance.GetGameProfileByConnection(Owner).team)
        //    mats[0].SetColor("_FresnelTint", new Color(1, 0, 0));
        //else
        //    mats[0].SetColor("_FresnelTint", Color.black);

        //carRenderer.sharedMaterials = mats;
    }
    [ServerRpc]
    private void AddProfile()
    {
        GameProfileManager.Instance.gameProfiles.Add(this);
    }
    [ServerRpc]
    private void RemoveProfile()
    {
        GameProfileManager.Instance.gameProfiles.Remove(this);
    }
    [ServerRpc]
    private void ChangeName(string name)
    {
        steamName.Value = name;
    }
}