using TMPro;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    public static DebugConsole Instance;
    [SerializeField] private TextMeshProUGUI m_ConsoleText;

    private void Awake()
    {
        Instance = this;
    }
    public void PrintToConsole(string message)
    {
        m_ConsoleText.text += message + "\n";
    }
    public void PrintErrorToConsole(string message)
    {
        m_ConsoleText.text += "<color=red>[ERROR] " + message + "</color>\n";
    }
    public void PrintWarningToConsole(string message)
    {
        m_ConsoleText.text += "<color=yellow>[WARNING] " + message + "</color>\n";
    }
}