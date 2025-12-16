using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeWeaponType { Shovel, Scythe }
public class MeleeWeapon : MonoBehaviour, IUsable
{
    [SerializeField] private MeleeWeaponType m_MeleeWeaponType;
    [SerializeField] private MeleeData m_MeleeData;

    private bool m_Swinging = false;
    private bool m_OnCooldown = false;

    private InventorySlot m_SlotBuffer;
    public InventorySlot slot
    {
        get { return m_SlotBuffer; }
        set { m_SlotBuffer = value; }
    }

    public void AltUnUse()
    {
        
    }
    public void AltUse()
    {
        
    }
    public void UnUse()
    {
        m_Swinging = false;
    }
    public void Use()
    {
        m_Swinging = true;
    }
    private void OnEnable()
    {
        m_Swinging = false;
    }
    private void Update()
    {
        if (m_Swinging)
        {
            if (m_OnCooldown)
                return;

            StaticMeleeWeapon.Instance.MeleeClient(m_MeleeWeaponType);
            GameProfile.Instance.StartCoroutine(Cooldown());
        }
    }
    private IEnumerator Cooldown()
    {
        m_OnCooldown = true;
        yield return new WaitForSeconds(m_MeleeData.hitRate);
        m_OnCooldown = false;
    }
}