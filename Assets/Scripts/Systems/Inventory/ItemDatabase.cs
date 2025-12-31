using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    [SerializeField] private string m_ItemsResourcePath;
    public List<InventoryItem> items;

    private void Awake()
    {
        Instance = this;
    }
#if UNITY_EDITOR
    [ContextMenu("Rebuild Database")]
    public void RebuildDatabase()
    {
        var items = Resources.LoadAll<InventoryItem>(m_ItemsResourcePath);

        this.items = new List<InventoryItem>(items);
    }
#endif

    public InventoryItem GetItem(string itemName)
    {
        InventoryItem item = items.FirstOrDefault(i => i.name == itemName);

        if (item == null)
        {
            print("Item " + itemName + " not found in database");
            return null;
        }

        return item;
    }
}