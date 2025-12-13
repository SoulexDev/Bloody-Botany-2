using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableSpawner : MonoBehaviour, IUsable
{
    public InventoryItem item;
    public Throwable throwable;

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
        
    }
    public void Use()
    {
        if (GameProfile.Instance.inventorySystem.RemoveItem(item))
        {
            Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);

            Instantiate(throwable, ray.origin, Quaternion.identity).Throw(ray.direction);
        }
    }
}