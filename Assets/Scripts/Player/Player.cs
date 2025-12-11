using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public PlayerController playerController;
    public InventorySystem inventorySystem;
    public CurrencySystem currencySystem;

    private void Awake()
    {
        Instance = this;
    }
}