using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> m_InventorySlots = new List<InventorySlot>();
    [SerializeField] private Transform m_ItemSpawnTransform;

    private IUsable m_CurrentUsable;
    private GameObject m_LastItem;
    private int m_LastIndex = -1;

    private void Update()
    {
        switch (Input.inputString)
        {
            case "1":
                SwitchItem(0);
                break;
            case "2":
                SwitchItem(1);
                break;
            case "3":
                SwitchItem(2);
                break;
            case "4":
                SwitchItem(3);
                break;
            case "5":
                SwitchItem(4);
                break;
            default:
                break;
        }

        if (m_CurrentUsable != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                m_CurrentUsable.Use();

                if (m_InventorySlots[m_LastIndex].item == null)
                {
                    UnequipAll();
                }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                m_CurrentUsable.UnUse();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                m_CurrentUsable.AltUse();
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                m_CurrentUsable.AltUnUse();
            }
        }
    }
    public void UnequipAll()
    {
        m_LastIndex = -1;

        if (m_LastItem)
            Destroy(m_LastItem);

        m_CurrentUsable = null;
    }
    private void SwitchItem(int itemIndex)
    {
        if (itemIndex >= m_InventorySlots.Count || m_LastIndex == itemIndex)
            return;

        InventoryItem item = m_InventorySlots[itemIndex].item;

        if (item == null)
            return;

        GameObject spawnedObj = Instantiate(item.itemPrefab, m_ItemSpawnTransform);

        if (spawnedObj.TryGetComponent(out IUsable usable))
            m_CurrentUsable = usable;

        if (m_LastItem)
            Destroy(m_LastItem);

        m_LastItem = spawnedObj;
        m_LastIndex = itemIndex;
    }
    public int AddItem(InventoryItem item, int count = 1)
    {
        InventorySlot slot = m_InventorySlots.FirstOrDefault(s => s.item == item);

        if (slot != null)
        {
            if (slot.itemCount + count > item.maxStack)
            {
                count = slot.itemCount + count - item.maxStack;

                slot.itemCount = item.maxStack;

                //look for a slot thats empty, or can hold the item. if neither of these exist, return the count
                if (m_InventorySlots.Any(s => s.item == null && (s.exclusiveItem == null || s.exclusiveItem == item)))
                    return AddItem(item, count);
                else
                    return count;
            }
            else
            {
                slot.itemCount += count;
                return 0;
            }
        }
        else
        {
            slot = m_InventorySlots.First(s => s.item == null && (s.exclusiveItem == item || s.exclusiveItem == null));

            if (slot == null)
                return count;
            else
            {
                slot.item = item;
                slot.itemCount += count;

                return 0;
            }
        }
    }
    public bool CanStoreItem(InventoryItem item, int count)
    {
        return m_InventorySlots.Any(s => 
        (s.item == item && s.itemCount + count <= s.item.maxStack) || 
        (s.item == null && (s.exclusiveItem == null || s.exclusiveItem == item)));
    }
    public bool RemoveItem(InventoryItem item, int count = 1)
    {
        InventorySlot slot = m_InventorySlots.FirstOrDefault(s => s.item == item);

        if (slot == null || slot.itemCount - count < 0)
            return false;

        slot.itemCount -= count;

        return true;
    }
}