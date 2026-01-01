using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabBar : MonoBehaviour
{
    [SerializeField] private List<Tab> m_Tabs;

    private void Awake()
    {
        m_Tabs.ForEach(t =>
        {
            t.tabBtn.onClick.AddListener(() =>
            {
                m_Tabs.ForEach(t=>t.tabObj.SetActive(false));
                t.tabObj.SetActive(true);
            });
        });
    }
    private void OnDestroy()
    {
        m_Tabs.ForEach(t=>t.tabBtn.onClick.RemoveAllListeners());
    }
}
[System.Serializable]
public class Tab
{
    public Button tabBtn;
    public GameObject tabObj;
}