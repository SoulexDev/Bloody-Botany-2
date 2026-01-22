using UnityEngine;

public class ViewInfo : MonoBehaviour
{
    public string activeInfoString;
    public string infoString => interactable.powerSource ? 
        (interactable.powerSource.activated.Value ? activeInfoString : "Power Required") : activeInfoString;
    public Interactable interactable;
}