using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Throwable
{
    [SerializeField] private Plant m_SpawnPlant;
    public override void OnImpact(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Planter planter) && !planter.inUse)
        {
            if (Instantiate(m_SpawnPlant, planter.plantSpawnPoint.position, Quaternion.identity).TryGetComponent(out Plant plant))
            {
                plant.Initialize(planter);
            }
            planter.inUse = true;
        }
        else
            return;

        base.OnImpact(collision);
    }
}