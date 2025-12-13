using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> m_InventorySlots = new List<InventorySlot>();
    [SerializeField] private Transform m_ItemSpawnTransform;

    [SerializeField] private GameObject m_GrayoutTop;
    [SerializeField] private GameObject m_GrayoutBottom;

    private IUsable m_CurrentUsable;
    private Dictionary<int, GameObject> m_SpawnedItems;
    private GameObject m_LastItem;
    private int m_LastIndex = -1;

    private bool m_BotanyMode = false;

    private void Awake()
    {
        m_SpawnedItems = new Dictionary<int, GameObject>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_BotanyMode = !m_BotanyMode;
            m_GrayoutTop.SetActive(m_BotanyMode);
            m_GrayoutBottom.SetActive(!m_BotanyMode);
            SwitchItem(m_BotanyMode ? 2 : 0);
        }
        switch (Input.inputString)
        {
            case "1":
                SwitchItem(m_BotanyMode ? 2 : 0);
                break;
            case "2":
                SwitchItem(m_BotanyMode ? 3 : 1);
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
        {
            m_LastItem.SetActive(false);
            m_LastItem = null;
        }

        m_CurrentUsable = null;

        ItemSelectBar.Instance.DeInitialize();
        m_InventorySlots.ForEach(s => s.SetCountTextEnabledState(true));
    }
    private void SwitchItem(int itemIndex)
    {
        if (itemIndex >= m_InventorySlots.Count || m_LastIndex == itemIndex)
            return;

        InventorySlot slot = m_InventorySlots[itemIndex];

        InventoryItem item = slot.item;

        if (item == null)
        {
            UnequipAll();
            return;
        }

        m_InventorySlots.ForEach(s => s.SetCountTextEnabledState(true));

        slot.SetCountTextEnabledState(false);

        if (m_LastItem)
            m_LastItem.SetActive(false);

        GameObject obj;

        if (m_SpawnedItems.ContainsKey(itemIndex))
        {
            obj = m_SpawnedItems[itemIndex];
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(item.itemPrefab, m_ItemSpawnTransform);
            m_SpawnedItems.Add(itemIndex, obj);
        }

        if (item.itemType == ItemType.Gun && obj.TryGetComponent(out Gun gun))
            ItemSelectBar.Instance.InitializeValues(slot.selectBarAnchor.position, gun.gunData.magazineSize, gun.ammoCount);
        else
            ItemSelectBar.Instance.InitializeValues(slot.selectBarAnchor.position, item.maxStack, slot.itemCount);

        if (obj.TryGetComponent(out IUsable usable))
        {
            usable.slot = slot;
            m_CurrentUsable = usable;
        }

        m_LastItem = obj;
        m_LastIndex = itemIndex;
    }
    public int AddItem(InventoryItem item, int count = 1)
    {
        InventorySlot slot = m_InventorySlots.FirstOrDefault(s => s.item == item && s.itemCount < s.item.maxStack);

        if (slot != null)
        {
            if (slot.itemCount + count > item.maxStack)
            {
                count = slot.itemCount + count - item.maxStack;

                slot.itemCount = item.maxStack;

                //look for a slot thats empty, or can hold the item. if neither of these exist, return the count
                return AddItem(item, count);
            }
            else
            {
                slot.itemCount += count;
                return 0;
            }
        }
        else
        {
            slot = m_InventorySlots.FirstOrDefault(s => s.item == null && 
            (s.exclusiveItemType == item.itemType || s.exclusiveItemType == ItemType.None));

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
        (s.item == null && (s.exclusiveItemType == item.itemType || s.exclusiveItemType == ItemType.None)));
    }
    public bool RemoveItem(InventoryItem item, int count = 1)
    {
        InventorySlot slot = m_InventorySlots.FirstOrDefault(s => s.item == item);

        if (slot == null || slot.itemCount - count < 0)
            return false;

        slot.itemCount -= count;

        if (slot.item == null)
        {
            int index = m_InventorySlots.IndexOf(slot);

            if (m_SpawnedItems.ContainsKey(index))
            {
                Destroy(m_SpawnedItems[index]);
                m_SpawnedItems.Remove(index);
            }
        }

        return true;
    }
    public bool RemoveItemFromSlot(InventorySlot slot, int count = 1)
    {
        if (slot == null || slot.itemCount - count < 0)
            return false;

        slot.itemCount -= count;

        if (slot.item == null)
        {
            int index = m_InventorySlots.IndexOf(slot);

            if (m_SpawnedItems.ContainsKey(index))
            {
                Destroy(m_SpawnedItems[index]);
                m_SpawnedItems.Remove(index);
            }
        }

        return true;
    }
}