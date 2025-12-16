using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_IndexText;
    [SerializeField] private TextMeshProUGUI m_LetterText;
    [SerializeField] private GameObject m_Display1;
    [SerializeField] private GameObject m_Display2;

    public void SetData(bool graphic2, Vector3 position, string itemName, string index)
    {
        transform.position = position;

        if (graphic2)
        {
            m_Display1.SetActive(false);
            m_Display2.SetActive(true);
            m_LetterText.text = index;
        }
        else
        {
            m_Display2.SetActive(false);
            m_Display1.SetActive(true);
            m_NameText.text = itemName;
            m_IndexText.text = index;
        }
    }
}