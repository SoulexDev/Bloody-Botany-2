using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFinder : MonoBehaviour
{
    public static CanvasFinder Instance;

    public Image healthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI statusText;
    public Image statusImage;

    public List<InventorySlot> inventorySlots;

    public GameObject grayoutTop;
    public GameObject grayoutBottom;
    public Transform selectAnchor1;
    public Transform selectAnchor2;
    public Transform topSelectBar;

    public TextMeshProUGUI topSelectText1;
    public TextMeshProUGUI topSelectText2;

    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI roundText;

    private void Awake()
    {
        Instance = this;
    }
}