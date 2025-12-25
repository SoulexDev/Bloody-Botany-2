using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject m_SelectFX;
    public void OnPointerEnter(PointerEventData eventData)
    {
        m_SelectFX.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        m_SelectFX.SetActive(false);
    }
}