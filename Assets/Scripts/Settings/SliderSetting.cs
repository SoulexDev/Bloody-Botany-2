using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetting : MonoBehaviour
{
    [SerializeField] private Slider m_Slider;
    [SerializeField] private float m_InitialValue;
    [SerializeField] private TextMeshProUGUI m_SliderText;
    public void UpdateValue()
    {
        m_SliderText.text = (m_Slider.value * 100).ToString("f0") + "%";
    }
}