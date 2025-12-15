using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour, IUsable
{
    public GunData gunData;
    [SerializeField] private GunType m_GunType;
    [SerializeField] private AudioSource m_Source;

    private bool m_Firing = false;
    private bool m_OnCooldown = false;

    [HideInInspector] public int ammoCount;

    private float m_Spread;

    private InventorySlot m_SlotBuffer;
    public InventorySlot slot
    {
        get { return m_SlotBuffer; }
        set { m_SlotBuffer = value; }
    }

    private void Awake()
    {
        ammoCount = gunData.magazineSize;
    }
    private void OnEnable()
    {
        m_Firing = false;
        Crosshair.Instance.SetCrosshairOuterState(true);
    }
    private void OnDisable()
    {
        Crosshair.Instance.SetCrosshairOuterState(false);
    }
    public virtual void Use()
    {
        m_Firing = true;
    }
    private void Update()
    {
        if (ammoCount == 0)
            return;

        m_Spread = Mathf.Lerp(m_Spread, GameProfile.Instance.playerController.isMoving ? 0.05f : 0, Time.deltaTime * 10);

        Crosshair.Instance.SetCrosshairRadius(m_Spread + gunData.spread);

        if (m_Firing)
        {
            if (m_OnCooldown)
                return;

            if (gunData.automatic)
            {
                Fire();
            }
            else
            {
                Fire();
                m_Firing = false;
            }
        }
    }
    private void Fire()
    {
        StaticGun.Instance.FireClient(m_GunType, m_Spread);

        m_Source.pitch = 1 + (Random.value - 0.5f) * 2 * 0.2f;
        m_Source.PlayOneShot(gunData.fireSound);

        GameProfile.Instance.StartCoroutine(Cooldown());

        ammoCount -= 1;

        SideSelectBar.Instance.UpdateValue(ammoCount);

        if (ammoCount <= 0)
        {
            GameProfile.Instance.StartCoroutine(Wilt());
        }
    }
    public virtual void UnUse()
    {
        m_Firing = false;
    }
    public virtual void AltUse()
    {
        
    }
    public virtual void AltUnUse()
    {
        
    }
    private IEnumerator Cooldown()
    {
        m_OnCooldown = true;
        yield return new WaitForSeconds(gunData.fireRate);
        m_OnCooldown = false;
    }
    private IEnumerator Wilt()
    {
        yield return new WaitForSeconds(2);

        if (GameProfile.Instance.inventorySystem.RemoveItemFromSlot(slot))
        {
            GameProfile.Instance.inventorySystem.UnequipAll();
        }
        else
        {
            Debug.LogError("Failed to remove gun from inventory. strange that its missing when you picked it up earlier..");
        }
    }
}