using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ViewInfo))]
public class Interactable : NetworkBehaviour
{
    [SerializeField] protected ViewInfo m_ViewInfo;

    public UnityEvent interactEvent;
    public bool isInteractable = true;

    public virtual void Awake()
    {
        m_ViewInfo = GetComponent<ViewInfo>();
    }
    public void Interact()
    {
        if (isInteractable)
            OnInteract();
    }
    public virtual void OnInteract()
    {
        interactEvent?.Invoke();
    }
}