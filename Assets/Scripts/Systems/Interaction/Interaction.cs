using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private Interactable m_CurrentInteractableBuffer;
    private Interactable m_CurrentInteractable
    {
        get { return m_CurrentInteractableBuffer; }
        set
        {
            m_CurrentInteractableBuffer = value;

            if (m_CurrentInteractableBuffer == null)
            {

            }
            else
            {

            }
        }
    }

    private ViewInfo m_CurrentViewInfoBuffer;
    private ViewInfo m_CurrentViewInfo
    {
        get { return m_CurrentViewInfoBuffer; }
        set
        {
            m_CurrentViewInfoBuffer = value;

            if (m_CurrentViewInfoBuffer == null)
                m_InfoText.text = "";
            else
            {
                if (m_CurrentViewInfoBuffer.interactable)
                {
                    m_InfoText.text = m_CurrentViewInfoBuffer.interactable.powerSource.activated.Value ? 
                        m_CurrentViewInfoBuffer.infoString : "Requires Power";
                }
                else
                    m_InfoText.text = m_CurrentViewInfoBuffer.infoString;
            }
        }
    }

    [SerializeField] private TextMeshProUGUI m_InfoText;

    private void Update()
    {
        ShootRays();
        PlayerInput();
    }
    public void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && m_CurrentInteractable != null)
        {
            m_CurrentInteractable.Interact();
        }
    }
    public void ShootRays()
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.SphereCast(ray, 0.05f, out RaycastHit hit, 2.5f, GameManager.playerIgnoreMask))
        {
            if (hit.transform.TryGetComponent(out Interactable newInteractable))
                m_CurrentInteractable = newInteractable;
            else
                m_CurrentInteractable = null;

            if (hit.transform.TryGetComponent(out ViewInfo newViewInfo))
                m_CurrentViewInfo = newViewInfo;
            else
                m_CurrentViewInfo = null;
        }
        else
        {
            m_CurrentInteractable = null;
            m_CurrentViewInfo = null;
        }
    }
}