using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour, IUsable
{
    [SerializeField] private GunData m_GunData;
    [SerializeField] private AudioSource m_Source;

    private bool m_Firing = false;
    private bool m_OnCooldown = false;

    private int m_AmmoCount;

    private void Awake()
    {
        m_AmmoCount = m_GunData.magazineSize;
    }
    public virtual void Use()
    {
        m_Firing = true;
    }
    private void Update()
    {
        if (m_AmmoCount == 0)
            return;

        if (m_Firing)
        {
            if (m_OnCooldown)
                return;

            if (m_GunData.automatic)
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
        for (int i = 0; i < m_GunData.bulletsPerShot; i++)
        {
            Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f + Vector2.one * Random.insideUnitCircle * m_GunData.spread);

            if (Physics.Raycast(ray, out RaycastHit hit, 999, GameManager.Instance.playerIgnoreMask))
            {
                if (hit.transform.TryGetComponent(out IHealth health))
                {
                    int finalDamage = Mathf.RoundToInt(m_GunData.damage * m_GunData.damageFalloff.Evaluate(hit.distance));
                    health.ChangeHealth(-finalDamage);
                }  
            }
        }

        m_Source.pitch = 1 + (Random.value - 0.5f) * 2 * 0.2f;
        m_Source.PlayOneShot(m_GunData.fireSound);
        StartCoroutine(Cooldown());

        m_AmmoCount -= 1;

        if (m_AmmoCount <= 0)
        {
            StartCoroutine(Wilt());
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
        yield return new WaitForSeconds(m_GunData.fireRate);
        m_OnCooldown = false;
    }
    private IEnumerator Wilt()
    {
        yield return new WaitForSeconds(2);

        if (Player.Instance.inventorySystem.RemoveItem(m_GunData.gunItem))
        {
            Player.Instance.inventorySystem.UnequipAll();
        }
        else
        {
            Debug.LogError("Failed to remove gun from inventory. strange that its missing when you picked it up earlier..");
        }
    }
}