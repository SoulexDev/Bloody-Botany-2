using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableSpawner : MonoBehaviour, IUsable
{
    [SerializeField] private ThrowableType m_ThrowableType;

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
        StaticThrowableSpawner.Instance.Throw(m_ThrowableType);
    }
}