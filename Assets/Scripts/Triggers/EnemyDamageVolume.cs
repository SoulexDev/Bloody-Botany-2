using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageVolume : MonoBehaviour
{
    public int damage = 5;

    private bool m_CanDamage = true;
    private void OnEnable()
    {
        m_CanDamage = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        //print(other);
        if (m_CanDamage && !other.CompareTag("Enemy") && other.transform.TryGetComponent(out IHealth health))
        {
            bool died = false;
            health.ChangeHealth(-damage, ref died);

            m_CanDamage = false;
        }
    }
}