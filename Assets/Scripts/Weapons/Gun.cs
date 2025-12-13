using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour, IUsable
{
    public GunData gunData;
    [SerializeField] private AudioSource m_Source;

    private bool m_Firing = false;
    private bool m_OnCooldown = false;

    public int ammoCount;

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

        m_Spread = Mathf.Lerp(m_Spread, Player.Instance.playerController.isMoving ? 0.05f : 0, Time.deltaTime * 10);

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
        for (int i = 0; i < gunData.bulletsPerShot; i++)
        {
            Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f + Vector2.one * Random.insideUnitCircle * (gunData.spread + m_Spread));

            if (Physics.Raycast(ray, out RaycastHit hit, 999, GameManager.Instance.playerIgnoreMask))
            {
                if (hit.transform.TryGetComponent(out IHealth health))
                {
                    int finalDamage = Mathf.RoundToInt(gunData.damage * gunData.damageFalloff.Evaluate(hit.distance));
                    health.ChangeHealth(-finalDamage);
                }  
            }
        }

        m_Source.pitch = 1 + (Random.value - 0.5f) * 2 * 0.2f;
        m_Source.PlayOneShot(gunData.fireSound);

        Player.Instance.StartCoroutine(Cooldown());

        ammoCount -= 1;

        ItemSelectBar.Instance.UpdateValue(ammoCount);

        if (ammoCount <= 0)
        {
            Player.Instance.StartCoroutine(Wilt());
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

        if (Player.Instance.inventorySystem.RemoveItemFromSlot(slot))
        {
            Player.Instance.inventorySystem.UnequipAll();
        }
        else
        {
            Debug.LogError("Failed to remove gun from inventory. strange that its missing when you picked it up earlier..");
        }
    }
}