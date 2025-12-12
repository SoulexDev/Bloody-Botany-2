using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageVolume : MonoBehaviour
{
    [SerializeField] private int m_Damage = 5;

    private bool m_CanDamage = true;
    private void OnEnable()
    {
        m_CanDamage = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        print(other);
        if (m_CanDamage && !other.CompareTag("Enemy") && other.transform.TryGetComponent(out IHealth health))
        {
            health.ChangeHealth(-m_Damage);

            m_CanDamage = false;
        }
    }
}