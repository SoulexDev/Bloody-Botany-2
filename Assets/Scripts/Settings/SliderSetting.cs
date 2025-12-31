using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetting : MonoBehaviour
{
    [SerializeField] private string m_SettingName;
    [SerializeField] private float m_InitialValue;
    [SerializeField] private Slider m_Slider;
    [SerializeField] private TextMeshProUGUI m_SliderText;
    private void OnEnable()
    {
        if (m_SettingName == string.Empty)
            return;

        m_Slider.value = PlayerPrefs.GetFloat(m_SettingName, m_InitialValue);
        UpdateText();
    }
    public void UpdateValue()
    {
        PlayerPrefs.SetFloat(m_SettingName, m_Slider.value);
        UpdateText();
    }
    private void UpdateText()
    {
        m_SliderText.text = (m_Slider.value * 100).ToString("f0") + "%";
    }
    [ContextMenu("Delete setting using setting name")]
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(m_SettingName);
    }
}