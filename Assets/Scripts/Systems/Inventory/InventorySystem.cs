using FishNet;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventorySystem : NetworkBehaviour
{
    private List<InventorySlot> m_InventorySlots => CanvasFinder.Instance.inventorySlots;
    private Transform m_ItemSpawnTransform => CameraController.Instance.itemSpawnTransform;

    private AmmoText m_AmmoText => CanvasFinder.Instance.ammoText;
    private SelectBar m_SelectBar => CanvasFinder.Instance.selectBar;

    private IUsable m_CurrentUsable;
    private Dictionary<int, GameObject> m_SpawnedItems;
    private GameObject m_LastItem;
    private int m_LastIndex = -1;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
            return;

        m_SpawnedItems = new Dictionary<int, GameObject>();
        m_InventorySlots.ForEach(s=>s.InitializeItems());

        SwitchItem(0);
    }
    private void Update()
    {
        if (!IsOwner || GameProfile.Instance.playerHealth.dead.Value)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchItem(0);
        }
        else if (Input.mouseScrollDelta.y != 0)
        {
            int direction = (Input.mouseScrollDelta.y > 0 ? -1 : 1);
            int nextIndex = m_LastIndex + direction;

            if (nextIndex < 0)
                nextIndex = 4;
            if (nextIndex > 4)
                nextIndex = 0;

            bool foundNext = false;
            while (!foundNext)
            {
                if (m_InventorySlots[nextIndex].item == null || m_InventorySlots[nextIndex].exclusiveItemType == ItemType.Throwable)
                {
                    nextIndex += direction;
                    if (nextIndex < 0)
                        nextIndex = 4;
                    if (nextIndex > 4)
                        nextIndex = 0;
                }
                else
                    foundNext = true;
            }

            SwitchItem(nextIndex);
        }
        else
        {
            int itemIndex = Input.inputString switch
            {
                "1" => 1,
                "2" => 2,
                "3" => 3,
                "4" => 4,
                _ => -1
            };
            SwitchItem(itemIndex);
        }
        if (m_CurrentUsable != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ItemType type = m_CurrentUsable.slot.item.itemType;
                m_CurrentUsable.Use();

                if (m_InventorySlots[m_LastIndex].item == null)
                {
                    Unequip(type);
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
    //TODO: melee fallback
    /// <summary>
    /// 
    /// </summary>
    /// <param name="toNextItem">Switches to next item instead of switching to melee</param>
    public void Unequip(ItemType itemType)
    {
        InventorySlot slot = m_InventorySlots.LastOrDefault(s => s.item != null && s.exclusiveItemType == itemType);

        if (slot != null)
            SwitchItem(m_InventorySlots.IndexOf(slot));
        else
        {
            slot = m_InventorySlots.LastOrDefault(s => s.item != null && s.exclusiveItemType != ItemType.Throwable);
            if (slot != null)
                SwitchItem(m_InventorySlots.IndexOf(slot));
        }
    }
    //TODO: Change select bar logic for item 0
    private void SwitchItem(int itemIndex)
    {
        //Check that item index is in bounds or itemIndex isnt the last switched index
        if (itemIndex < 0 || itemIndex >= m_InventorySlots.Count || m_LastIndex == itemIndex)
            return;

        //Get the slot and item
        InventorySlot slot = m_InventorySlots[itemIndex];
        InventoryItem item = slot.item;

        //Return if item is null
        if (item == null)
        {
            //SwitchItem(0);
            return;
        }

        //Initialize the select bar and set the slot selection states
        string index = itemIndex switch
        {
            0 => "Q",
            1 => "1",
            2 => "2",
            3 => "3",
            4 => "4",
            _ => ""
        };
        m_SelectBar.SetData(itemIndex == 0, slot.selectBarAnchor.position, item.name, index);

        //'Remove' the last item
        if (m_LastItem)
            m_LastItem.SetActive(false);

        //Get the item object
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

        //Initialize ammo text
        if (item.itemType == ItemType.Gun && obj.TryGetComponent(out Gun gun))
            AmmoText.Instance.InitializeValues(gun.gunData.magazineSize, gun.ammoCount);
        else if (itemIndex != 0)
            AmmoText.Instance.InitializeValues(item.maxStack, slot.itemCount);
        else
            AmmoText.Instance.DeInitialize();

        //Get usable from item object
        if (obj.TryGetComponent(out IUsable usable))
        {
            usable.slot = slot;
            m_CurrentUsable = usable;
        }

        //Set last obj and last index
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
    public bool HasItem(InventoryItem item, int count = 1)
    {
        InventorySlot slot = m_InventorySlots.FirstOrDefault(s => s.item == item);

        if (slot == null || slot.itemCount - count < 0)
            return false;

        return true;
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
            if (m_LastIndex == index)
            {
                Unequip(item.itemType);
            }
        }

        return true;
    }

    public bool RemoveItemFromSlot(InventorySlot slot, int count = 1)
    {
        if (slot == null || slot.itemCount - count < 0)
            return false;

        ItemType type = slot.item.itemType;
        slot.itemCount -= count;

        if (slot.item == null)
        {
            int index = m_InventorySlots.IndexOf(slot);

            if (m_SpawnedItems.ContainsKey(index))
            {
                Destroy(m_SpawnedItems[index]);
                m_SpawnedItems.Remove(index);
            }
            if (m_LastIndex == index)
            {
                Unequip(type);
            }
        }

        return true;
    }
    public InventorySlot GetSlotFromIndex(int index)
    {
        return m_InventorySlots[index];
    }
    public int GetIndexFromSlot(InventorySlot slot)
    {
        return m_InventorySlots.IndexOf(slot);
    }
}