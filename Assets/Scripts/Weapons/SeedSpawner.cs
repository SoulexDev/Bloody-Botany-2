using UnityEngine;

public class SeedSpawner : MonoBehaviour, IUsable
{
    [SerializeField] private SeedType m_SeedType;

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
        //StaticSeedSpawner.Instance.TryThrowClient(m_SeedType);
    }
    public void Use()
    {
        StaticSeedSpawner.Instance.TryThrowClient(m_SeedType, GameProfile.Instance.inventorySystem.GetIndexFromSlot(slot));
    }
}