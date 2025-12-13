using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientGrenade : Throwable
{
    [SerializeField] private GameObject m_SplashEffect;
    public override void OnImpact(Collision collision)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 1);

        foreach (Collider col in cols)
        {
            if (col.TryGetComponent(out Plant plant))
            {
                plant.Feed();
            }
        }
        if (m_SplashEffect)
            Instantiate(m_SplashEffect, transform.position, Quaternion.identity);
        base.OnImpact(collision);
    }
}