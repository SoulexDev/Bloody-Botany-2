using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private HealthComponent m_HealthComponent;
    [SerializeField] private Sprite m_ShieldIcon;
    [SerializeField] private Sprite m_WarningIcon;
    [SerializeField] private GameObject m_ReviveObj;
    private TextMeshProUGUI m_StatusText => CanvasFinder.Instance.statusText;
    private Image m_StatusImage => CanvasFinder.Instance.statusImage;
    private Image m_HealthBar => CanvasFinder.Instance.healthBar;
    private TextMeshProUGUI m_HealthText => CanvasFinder.Instance.healthText;

    [AllowMutableSyncType] public SyncVar<bool> dead = new SyncVar<bool>();

    private bool m_ShieldedBuffer;
    private bool m_Shielded
    {
        get { return m_ShieldedBuffer; }
        set
        {
            m_ShieldedBuffer = value;

            m_StatusImage.sprite = m_ShieldedBuffer ? m_ShieldIcon : m_WarningIcon;
            m_StatusText.text = m_ShieldedBuffer ? "Protected" : "Unprotected";
        }
    }

    private float m_DamageTimer;
    private float m_HealTimer;

    private float m_HealingPerkValue;
    private float m_DamagePerkValue;

    //private float m_OxygenValue;

    private void Start()
    {
        //TODO: place perks manager as a reference or place in game profile
        m_HealthComponent.OnHealthChanged += HealthComponent_OnHealthChanged;
        m_HealthComponent.OnHealthDepleted += HealthComponent_OnHealthDepleted;

        PerksManager.OnPerksChanged += PerksManager_OnPerksChanged;
    }
    private void OnDestroy()
    {
        m_HealthComponent.OnHealthChanged -= HealthComponent_OnHealthChanged;
        m_HealthComponent.OnHealthDepleted -= HealthComponent_OnHealthDepleted;

        PerksManager.OnPerksChanged -= PerksManager_OnPerksChanged;
    }
    private void PerksManager_OnPerksChanged()
    {
        m_HealingPerkValue = 1.5f / PerksManager.Instance.GetPerkValue(PerkType.Speed_Healing, 1);
        m_DamagePerkValue = PerksManager.Instance.GetPerkValue(PerkType.LungCapacity, 1);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
            return;

        m_DamageTimer = 1;
        m_HealTimer = 1.5f;

        m_HealingPerkValue = 1.5f;
        m_DamagePerkValue = 1;

        m_Shielded = false;
    }
    private void HealthComponent_OnHealthChanged()
    {
        if (!IsOwner)
            return;

        if (m_HealthBar)
            m_HealthBar.fillAmount = (float)m_HealthComponent.health / m_HealthComponent.maxHealth;
        if (m_HealthText)
            m_HealthText.text = m_HealthComponent.health.ToString("D2") + "/" + m_HealthComponent.maxHealth.ToString("D2");
    }
    private void HealthComponent_OnHealthDepleted()
    {
        SetDeadStateServer(true);
        SetReviveActiveStateServer(true);
    }
    //TODO: Add in specific callback function to IHealth, rather than forcing everything to use ref bool
    private void Update()
    {
        if (!IsOwner || dead.Value)
            return;

        bool died = false;
        if (!m_Shielded)
        {
            if (m_DamageTimer <= 0)
            {
                m_DamageTimer += m_DamagePerkValue;
                m_HealthComponent.ChangeHealth(-1, ref died);
            }

            m_DamageTimer -= Time.deltaTime;
        }
        else
        {
            if (m_HealTimer <= 0)
            {
                m_HealTimer += m_HealingPerkValue;
                m_HealthComponent.ChangeHealth(1, ref died);
            }

            m_HealTimer -= Time.deltaTime;
        }

        //m_OxygenValue = Mathf.MoveTowards(m_OxygenValue, m_Shielded ? 100 : 10, Time.deltaTime * 50);

        //m_OxygenText.text = m_OxygenValue.ToString("f0");
    }
    public void SetShieldedState(bool state)
    {
        m_Shielded = state;
        //print(m_Shielded);
    }
    [ServerRpc]
    private void SetDeadStateServer(bool state)
    {
        dead.Value = state;
    }
    public void Revive()
    {
        SetDeadStateServer(false);
        m_HealthComponent.health = m_HealthComponent.maxHealth / 2;

        SetReviveActiveStateServer(false);
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetReviveActiveStateServer(bool state)
    {
        SetReviveActiveStateClients(state);
    }
    [ObserversRpc]
    public void SetReviveActiveStateClients(bool state)
    {
        m_ReviveObj.SetActive(state);
    }
}