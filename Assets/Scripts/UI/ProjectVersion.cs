using TMPro;
using UnityEngine;

public class ProjectVersion : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().text = Application.version;
    }
}
