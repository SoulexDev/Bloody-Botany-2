using FishNet.Connection;
using FishNet.Object;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticMeleeWeapon : NetworkBehaviour
{
    public static StaticMeleeWeapon Instance;

    [SerializeField] private List<MeleeWeaponTypeDataPair> m_MeleeTypeDataPairs;
    [SerializeField] private InventoryItem m_NutrientGrenade;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            Instance = this;
        }
    }
    public void MeleeClient(MeleeWeaponType meleeWeaponType)
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);

        MeleeServer(meleeWeaponType, ray.origin, ray.direction, Owner);
    }
    [ServerRpc]
    public void MeleeServer(MeleeWeaponType meleeWeaponType, Vector3 origin, Vector3 direction, NetworkConnection conn)
    {
        MeleeData data = m_MeleeTypeDataPairs.First(d=>d.meleeWeaponType == meleeWeaponType).meleeData;

        Ray ray = new Ray(origin, direction);
        if (Physics.SphereCast(ray, 0.5f, out RaycastHit hit, 2, GameManager.Instance.playerIgnoreMask))
        {
            Collider[] cols = Physics.OverlapSphere(hit.point, 2, GameManager.Instance.playerIgnoreMask);

            foreach (Collider col in cols)
            {
                if (col.CompareTag("Enemy") && col.TryGetComponent(out IHealth health))
                {
                    bool died = false;
                    health.ChangeHealth(-data.damage, ref died);

                    ClientCallback(conn, died);
                }
            }
        }
    }
    [TargetRpc]
    private void ClientCallback(NetworkConnection conn, bool died)
    {
        if (died)
        {
            GameProfile.Instance.currencySystem.AddCurrency(Random.Range(20, 100));
            GameProfile.Instance.inventorySystem.AddItem(m_NutrientGrenade, 1);
        }
    }
}
[System.Serializable]
public class MeleeWeaponTypeDataPair
{
    public MeleeWeaponType meleeWeaponType;
    public MeleeData meleeData;
}