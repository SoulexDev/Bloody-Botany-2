using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour, IUsable
{
    public GunData gunData;
    [SerializeField] private GunType m_GunType;
    [SerializeField] private AudioSource m_Source;
    [HideInInspector] public int ammoCount;

    private bool m_Firing = false;
    private bool m_OnCooldown = false;

    private float m_Spread;

    private InventorySlot m_SlotBuffer;
    public InventorySlot slot
    {
        get { return m_SlotBuffer; }
        set { m_SlotBuffer = value; }
    }

    private void Start()
    {
        ammoCount = Mathf.RoundToInt(gunData.magazineSize * StatsManager.Instance.accuracyMagMult);
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
    private void Update()
    {
        if (ammoCount == 0)
            return;

        float spreadUpper = (0.05f + gunData.spread) / StatsManager.Instance.accuracyMagMult;
        float spreadLower = gunData.spread / StatsManager.Instance.accuracyMagMult;

        m_Spread = Mathf.Lerp(m_Spread, GameProfile.Instance.playerController.isMoving ? spreadUpper : spreadLower, Time.deltaTime * 10);

        Crosshair.Instance.SetCrosshairRadius(m_Spread);

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
    public virtual void Use()
    {
        m_Firing = true;
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
    private void Fire()
    {
        StaticGun.Instance.FireClient(m_GunType, m_Spread, StatsManager.Instance.damageFiringMult * gunData.damage);
        GameProfile.Instance.inventorySystem.DoFireAnimation();

        m_Source.pitch = 1 + (Random.value - 0.5f) * 2 * 0.2f;
        m_Source.PlayOneShot(gunData.fireSound);

        GameProfile.Instance.StartCoroutine(Cooldown());

        ammoCount -= 1;

        AmmoText.Instance.UpdateValue(ammoCount);

        if (ammoCount <= 0)
        {
            Crosshair.Instance.SetCrosshairOuterState(false);
            GameProfile.Instance.StartCoroutine(Wilt());
        }
    }
    private IEnumerator Cooldown()
    {
        m_OnCooldown = true;
        yield return new WaitForSeconds(gunData.fireRate / StatsManager.Instance.damageFiringMult);
        m_OnCooldown = false;
    }
    private IEnumerator Wilt()
    {
        yield return new WaitForSeconds(2);

        if (GameProfile.Instance.inventorySystem.RemoveItemFromSlot(slot))
        {
            //GameProfile.Instance.inventorySystem.UnequipSlot(slot);
        }
        else
        {
            Debug.LogError("Failed to remove gun from inventory. strange that its missing when you picked it up earlier..");
        }
    }
}