using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientGrenade : Throwable
{
    [SerializeField] private InventoryItem m_Item;
    [SerializeField] private GameObject m_SplashEffect;
    [SerializeField] private float m_EffectRadius = 2;
    public override void OnImpact(Collision collision)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, m_EffectRadius);

        foreach (Collider col in cols)
        {
            if (col.TryGetComponent(out Plant plant))
            {
                plant.Feed();
            }
            if (col.TryGetComponent(out IHealth health) && col.CompareTag("Enemy"))
            {
                bool died = false;
                health.ChangeHealth(-2, ref died);

                //TODO: Rename all these client callback functions
                if (died)
                    ImpactClientCallback(Owner);
            }
        }
        if (m_SplashEffect)
            SpawnEffectsClient(transform.position);

        base.OnImpact(collision);
    }
    [ObserversRpc]
    private void SpawnEffectsClient(Vector3 position)
    {
        Instantiate(m_SplashEffect, position, Quaternion.identity);
    }
    //TODO: Still.. an item database
    [TargetRpc]
    private void ImpactClientCallback(NetworkConnection conn)
    {
        GameProfile.Instance.currencySystem.AddCurrency(Random.Range(20, 100));
        GameProfile.Instance.inventorySystem.AddItem(m_Item, 1);
    }
}