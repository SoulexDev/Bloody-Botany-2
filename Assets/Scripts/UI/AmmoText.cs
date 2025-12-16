using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoText : MonoBehaviour
{
    public static AmmoText Instance;

    [SerializeField] private TextMeshProUGUI m_DisplayText;

    private int m_MaxValue;

    private void Awake()
    {
        Instance = this;
    }
    public void InitializeValues(int maxValue, int currentValue)
    {
        m_MaxValue = maxValue;

        m_DisplayText.text = currentValue.ToString("D2") + "/" + m_MaxValue.ToString("D2");
        m_DisplayText.enabled = true;
    }
    public void UpdateValue(int value)
    {
        m_DisplayText.text = value.ToString("D2") + "/" + m_MaxValue.ToString("D2");
    }
    public void DeInitialize()
    {
        m_DisplayText.enabled = false;
    }
}