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
    public void TickLeft()
    {
        optionIndex--;

        if (optionIndex < 0)
            optionIndex = options.Length - 1;

        m_OptionText.text = options[optionIndex];
    }
    public void TickRight()
    {
        optionIndex++;

        if (optionIndex >= options.Length)
            optionIndex = 0;

        m_OptionText.text = options[optionIndex];
    }
}