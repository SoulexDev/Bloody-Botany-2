using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { None, Throwable, Seed, Gun }

[CreateAssetMenu(menuName = "Bloody Botany/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public ItemType itemType;
    public Sprite itemSprite;
    public GameObject itemPrefab;
    public int maxStack = 1;

    public string equipAnimationTrigger;
}