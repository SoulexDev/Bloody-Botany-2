using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TickSetting : MonoBehaviour
{
    [SerializeField] private string m_SettingName;
    [SerializeField] private int m_DefaultOptionIndex = 0;
    [SerializeField] private TextMeshProUGUI m_OptionText;

    public string[] options;
    private int optionIndex = 0;
    private void OnEnable()
    {
        if (m_SettingName == string.Empty)
            return;

        optionIndex = PlayerPrefs.GetInt(m_SettingName, m_DefaultOptionIndex);
        UpdateText();
    }
    public void TickLeft()
    {
        optionIndex--;

        if (optionIndex < 0)
            optionIndex = options.Length - 1;

        PlayerPrefs.SetInt(m_SettingName, optionIndex);
        UpdateText();
    }
    public void TickRight()
    {
        optionIndex++;

        if (optionIndex >= options.Length)
            optionIndex = 0;

        PlayerPrefs.SetInt(m_SettingName, optionIndex);
        UpdateText();
    }
    private void UpdateText()
    {
        m_OptionText.text = options[optionIndex];
    }
    [ContextMenu("Delete setting using setting name")]
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(m_SettingName);
    }
}