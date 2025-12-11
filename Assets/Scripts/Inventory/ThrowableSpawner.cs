using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableSpawner : MonoBehaviour, IUsable
{
    public InventoryItem item;
    public Throwable throwable;

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
        if (Player.Instance.inventorySystem.RemoveItem(item))
        {
            Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);

            Instantiate(throwable, ray.origin, Quaternion.identity).Throw(ray.direction);
        }
    }
}