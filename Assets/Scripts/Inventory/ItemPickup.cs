using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public InventoryItem item;
    public int itemCount = 1;

    public override void Awake()
    {
        base.Awake();

        m_ViewInfo.infoString = $"Pick up {item.name}";
    }
    public override void OnInteract()
    {
        base.OnInteract();

        int returnAmount = GameProfile.Instance.inventorySystem.AddItem(item, itemCount);

        if (returnAmount == 0)
        {
            OnPickupSuccess();
            Destroy(gameObject);
        }
        else
            itemCount = returnAmount;
    }
    public virtual void OnPickupSuccess()
    {

    }
}