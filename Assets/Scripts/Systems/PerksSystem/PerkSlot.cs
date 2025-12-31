using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_CountText;
    [SerializeField] private Image m_PerkImage;

    private Perk m_PerkBuffer;
    public Perk perk
    {
        get { return m_PerkBuffer; }
        set
        {
            m_PerkBuffer = value;

            m_PerkImage.sprite = m_PerkBuffer ? m_PerkBuffer.perkIcon : null;
            m_PerkImage.enabled = m_PerkBuffer != null;
        }
    }
    private int m_PerkCountBuffer;
    public int perkCount
    {
        get { return m_PerkCountBuffer; }
        set
        {
            m_PerkCountBuffer = value;

            if (m_PerkCountBuffer <= 0)
                perk = null;

            m_CountText.text = m_PerkCountBuffer > 0 ? $"x{m_PerkCountBuffer}" : "";
        }
    }
}