using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ViewInfo))]
public class Interactable : NetworkBehaviour
{
    [SerializeField] protected ViewInfo m_ViewInfo;
    public PowerGenerator powerSource;

    public UnityEvent interactEvent;
    public bool isInteractable = true;

    public virtual void Awake()
    {
        m_ViewInfo = GetComponent<ViewInfo>();

        m_ViewInfo.interactable = this;
    }
    public void Interact()
    {
        if (powerSource != null && !powerSource.activated.Value)
            return;

        if (isInteractable)
            OnInteract();
    }
    public virtual void OnInteract()
    {
        interactEvent?.Invoke();
    }
}