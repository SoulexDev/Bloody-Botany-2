using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public InventoryItem exclusiveItem;

    private int m_ItemCountBuffer;
    public int itemCount
    {
        get { return m_ItemCountBuffer; }
        set
        {
            m_ItemCountBuffer = value;
            if (m_ItemCountBuffer <= 0)
            {
                item = null;
                m_ItemCountBuffer = 0;
            }
        }
    }
    public InventoryItem item;
}