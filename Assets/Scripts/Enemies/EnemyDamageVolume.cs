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
            bool died = false;
            health.ChangeHealth(-m_Damage, ref died);

            m_CanDamage = false;
        }
    }
}