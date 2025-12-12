using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBubble : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.TryGetComponent(out PlayerHealth health))
            {
                health.SetShieldedState(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.TryGetComponent(out PlayerHealth health))
            {
                health.SetShieldedState(false);
            }
        }
    }
}