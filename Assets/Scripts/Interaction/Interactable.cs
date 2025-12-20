using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ViewInfo))]
public class Interactable : NetworkBehaviour
{
    [SerializeField] protected ViewInfo m_ViewInfo;
    [SerializeField] protected PowerGenerator m_PowerSource;

    public UnityEvent interactEvent;
    public bool isInteractable = true;

    public virtual void Awake()
    {
        m_ViewInfo = GetComponent<ViewInfo>();
    }
    public void Interact()
    {
        if (m_PowerSource != null && !m_PowerSource.activated.Value)
            return;

        if (isInteractable)
            OnInteract();
    }
    public virtual void OnInteract()
    {
        interactEvent?.Invoke();
    }
}