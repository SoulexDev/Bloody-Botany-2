using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSelectBar : MonoBehaviour
{
    public static ItemSelectBar Instance;

    [SerializeField] private GameObject m_Display;
    [SerializeField] private TextMeshProUGUI m_DisplayText;

    private int m_MaxValue;

    private void Awake()
    {
        Instance = this;
    }
    public void InitializeValues(Vector3 position, int maxValue, int currentValue)
    {
        transform.position = position;
        m_MaxValue = maxValue;

        m_DisplayText.text = currentValue.ToString("D2") + "/" + m_MaxValue.ToString("D2");
        m_Display.SetActive(true);
    }
    public void UpdateValue(int value)
    {
        m_DisplayText.text = value.ToString("D2") + "/" + m_MaxValue.ToString("D2");
    }
    public void DeInitialize()
    {
        m_Display.SetActive(false);
    }
}