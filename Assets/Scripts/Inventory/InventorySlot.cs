using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image m_IconImage;
    [SerializeField] private TextMeshProUGUI m_CountText;
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private InventoryItem m_InitialItem;

    public ItemType exclusiveItemType;
    public Transform selectBarAnchor;
    //[SerializeField] private bool m_DisableCountTextWhenSelected = false;

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
            if (m_CountText)
                m_CountText.text = m_ItemCountBuffer > 0 ? $"x{m_ItemCountBuffer}" : "";
        }
    }
    private InventoryItem m_ItemBuffer;
    public InventoryItem item
    {
        get { return m_ItemBuffer; }
        set
        {
            m_ItemBuffer = value;

            if (m_ItemBuffer && m_ItemBuffer.itemSprite)
            {
                m_IconImage.sprite = m_ItemBuffer.itemSprite;
                m_IconImage.enabled = true;
            }
            else
            {
                m_IconImage.sprite = null;
                m_IconImage.enabled = false;
            }
            if (m_NameText)
                m_NameText.text = item?.name;
        }
    }
    public void InitializeItems()
    {
        if (m_InitialItem)
        {
            item = m_InitialItem;
        }
    }
    public void SetSelectedState(bool state)
    {
        //m_NameText.color = state ? m_SelectedColor : Color.white;

        //if (!m_DisableCountTextWhenSelected || !m_CountText)
        //    return;

        //m_CountText.enabled = state;
    }
}