using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public enum ShieldState { Inactive, Active, Broken, Locked }
public class Shield : Interactable, IHealth
{
    [SerializeField] private ShieldState m_StartStatus;
    [AllowMutableSyncType] public SyncVar<ShieldState> shieldState = new SyncVar<ShieldState>();
    public delegate void ShieldStateChanged();
    public event ShieldStateChanged OnShieldStateChanged;

    [SerializeField] private int m_MaxHealth = 100;
    [SerializeField] private int m_RepairCost;
    [SerializeField] private GameObject m_ShieldVolume;
    private int m_Health;

    public override void Awake()
    {
        base.Awake();

        shieldState.OnChange += ShieldState_OnChange;

        UpdateInteractionText();
    }
    public override void OnStartServer()
    {
        base.OnStartServer();

        shieldState.Value = m_StartStatus;
    }
    private void OnDestroy()
    {
        shieldState.OnChange -= ShieldState_OnChange;
    }
    private void ShieldState_OnChange(ShieldState prev, ShieldState next, bool asServer)
    {
        if (prev != next)
        {
            UpdateInteractionText();

            OnShieldStateChanged?.Invoke();
            m_ShieldVolume.SetActive(next == ShieldState.Active);
        }
    }
    public void ChangeHealth(int amount, ref bool died)
    {
        if (!IsServerInitialized)
            return;

        if (shieldState.Value != ShieldState.Active)
            return;

        m_Health += amount;

        print(m_Health);

        if (m_Health <= 0)
        {
            shieldState.Value = ShieldState.Broken;
        }
    }
    //TODO: Make the player interaction send RPCs instead of sending RPCs per object
    //TODO: Stop being a tard and make this more server authoritative with client callbacks you FUCKING TROGLYTARD
    public override void OnInteract()
    {
        base.OnInteract();

        switch (shieldState.Value)
        {
            case ShieldState.Inactive:
                HandleInactiveServer();
                break;
            case ShieldState.Active:
                HandleActiveServer();
                break;
            case ShieldState.Broken:
                if (GameProfile.Instance.currencySystem.SpendCurrency(m_RepairCost))
                {
                    HandleBrokenServer();
                }
                break;
            case ShieldState.Locked:
                HandleLockedServer();
                break;
            default:
                break;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void HandleInactiveServer()
    {
        if (ShieldManager.Instance.TrySetCurrentShield(this))
            shieldState.Value = ShieldState.Active;
        else
            print("Cant set shield");
    }
    [ServerRpc(RequireOwnership = false)]
    private void HandleActiveServer()
    {
        shieldState.Value = ShieldState.Locked;
    }
    [ServerRpc(RequireOwnership = false)]
    private void HandleBrokenServer()
    {
        if (ShieldManager.Instance.TrySetCurrentShield(this))
            shieldState.Value = ShieldState.Active;
        else
            shieldState.Value = ShieldState.Inactive;

        m_Health = m_MaxHealth;
    }
    [ServerRpc(RequireOwnership = false)]
    private void HandleLockedServer()
    {
        if (ShieldManager.Instance.TrySetCurrentShield(this))
            shieldState.Value = ShieldState.Active;
        else
            print("Cant set shield");
    }
    private void UpdateInteractionText()
    {
        m_ViewInfo.infoString = shieldState.Value switch
        {
            ShieldState.Inactive => "Activate Shield",
            ShieldState.Active => "Lockdown Shield",
            ShieldState.Broken => $"Repair Shield\n${m_RepairCost}",
            ShieldState.Locked => "Unlock Shield",
            _ => ""
        };
    }
}