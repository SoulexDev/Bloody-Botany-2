using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Rb;
    [SerializeField] private float m_ThrowForce = 16;

    public void Throw(Vector3 direction)
    {
        m_Rb.AddForce(direction * m_ThrowForce, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        OnImpact(collision);
    }
    public virtual void OnImpact(Collision collision)
    {
        Destroy(gameObject);
    }
}