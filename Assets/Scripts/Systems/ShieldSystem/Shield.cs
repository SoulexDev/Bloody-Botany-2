using FishNet.CodeGenerating;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.UI;

public enum ShieldState { Inactive, Active, Broken, Locked }
public class Shield : Interactable, IHealth
{
    [SerializeField] private ShieldState m_StartStatus;
    [AllowMutableSyncType] public SyncVar<ShieldState> shieldState = new SyncVar<ShieldState>();
    public delegate void ShieldStateChanged();
    public event ShieldStateChanged OnShieldStateChanged;

    [SerializeField] private int m_MaxHealth = 250;
    [SerializeField] private int m_RepairCost;
    [SerializeField] private GameObject m_ShieldVolume;
    [SerializeField] private Image m_HealthBar;
    [AllowMutableSyncType] private SyncVar<int> m_Health = new SyncVar<int>();

    public override void Awake()
    {
        base.Awake();

        shieldState.OnChange += ShieldState_OnChange;
        m_Health.OnChange += Health_OnChange;

        UpdateInteractionText();
    }
    private void OnDestroy()
    {
        shieldState.OnChange -= ShieldState_OnChange;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();

        m_Health.Value = m_MaxHealth;
        shieldState.Value = m_StartStatus;
        if (shieldState.Value == ShieldState.Active)
        {
            ShieldManager.Instance.TrySetCurrentShield(this);
        }
    }
    private void ShieldState_OnChange(ShieldState prev, ShieldState next, bool asServer)
    {
        UpdateInteractionText();

        OnShieldStateChanged?.Invoke();
        m_ShieldVolume.SetActive(next == ShieldState.Active);
    }
    private void Health_OnChange(int prev, int next, bool asServer)
    {
        m_HealthBar.fillAmount = (float)next / m_MaxHealth;
    }
    public void ChangeHealth(int amount, ref bool died)
    {
        if (!IsServerInitialized)
            return;

        if (shieldState.Value != ShieldState.Active)
            return;

        m_Health.Value += amount;

        if (m_Health.Value <= 0)
        {
            shieldState.Value = ShieldState.Broken;
        }
    }
    public override void OnInteract()
    {
        base.OnInteract();

        HandleInteractServer(LocalConnection, canSpendCurrency: GameProfile.Instance.currencySystem.HasCurrency(m_RepairCost));
    }
    //TODO: Apparently, fishnet already sends the network connection so long as you put the parameter and do = null... WRONG. It didnt work, probably just gotta try another time
    [ServerRpc(RequireOwnership = false)]
    private void HandleInteractServer(NetworkConnection conn, Channel channel = Channel.Unreliable, bool canSpendCurrency = false)
    {
        switch (shieldState.Value)
        {
            case ShieldState.Inactive:
                if (ShieldManager.Instance.TrySetCurrentShield(this))
                    shieldState.Value = ShieldState.Active;
                else
                    print("Cant set shield");
                break;
            case ShieldState.Active:
                shieldState.Value = ShieldState.Locked;
                break;
            case ShieldState.Broken:
                if (canSpendCurrency)
                {
                    if (ShieldManager.Instance.TrySetCurrentShield(this))
                        shieldState.Value = ShieldState.Active;
                    else
                        shieldState.Value = ShieldState.Inactive;

                    m_Health.Value = m_MaxHealth;

                    SpentClientCallback(conn);
                }
                break;
            case ShieldState.Locked:
                if (ShieldManager.Instance.TrySetCurrentShield(this))
                    shieldState.Value = ShieldState.Active;
                else
                    print("Cant set shield");
                break;
            default:
                break;
        }
    }
    [TargetRpc]
    private void SpentClientCallback(NetworkConnection conn, Channel channel = Channel.Unreliable)
    {
        GameProfile.Instance.currencySystem.SpendCurrency(m_RepairCost);
    }
    private void UpdateInteractionText()
    {
        m_ViewInfo.activeInfoString = shieldState.Value switch
        {
            ShieldState.Inactive => "Activate Shield",
            ShieldState.Active => "Lockdown Shield",
            ShieldState.Broken => $"Repair Shield\n${m_RepairCost}",
            ShieldState.Locked => "Unlock Shield",
            _ => ""
        };
    }
}