using FishNet;
using FishNet.Object;
using UnityEngine;

public class Throwable : NetworkBehaviour
{
    [SerializeField] private Rigidbody m_Rb;
    [SerializeField] private Collider m_Col;
    [SerializeField] private float m_ThrowForce = 16;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsServerInitialized)
        {
            m_Rb.isKinematic = true;
            m_Col.enabled = false;
        }
    }
    public void Throw(Vector3 direction)
    {
        m_Rb.AddForce(direction * m_ThrowForce, ForceMode.Impulse);
    }
    [Server]
    private void OnCollisionEnter(Collision collision)
    {
        OnImpact(collision);
    }
    public virtual void OnImpact(Collision collision)
    {
        InstanceFinder.ServerManager.Despawn(gameObject);
    }
}