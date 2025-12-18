using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
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
    public void MeleeServer(MeleeWeaponType meleeWeaponType, Vector3 origin, Vector3 direction, NetworkConnection conn, Channel channel = Channel.Unreliable)
    {
        MeleeData data = m_MeleeTypeDataPairs.First(d=>d.meleeWeaponType == meleeWeaponType).meleeData;
        Collider[] cols = Physics.OverlapSphere(origin + direction * 1.5f, 2, GameManager.Instance.playerIgnoreMask);

        int sweepTotal = 0;

        foreach (Collider col in cols)
        {
            if (sweepTotal >= data.sweepCount)
                break;

            if (col.CompareTag("Enemy") && col.TryGetComponent(out IHealth health))
            {
                sweepTotal++;

                bool died = false;
                health.ChangeHealth(-data.damage, ref died);

                ClientCallback(conn, died);
            }
        }
    }
    [TargetRpc]
    private void ClientCallback(NetworkConnection conn, bool died, Channel channel = Channel.Unreliable)
    {
        if (died)
        {
            GameProfile.Instance.currencySystem.AddCurrency(Random.Range(
            GameManager.Instance.difficultySettings.onKillPaymentLowerBound,
            GameManager.Instance.difficultySettings.onKillPaymentUpperBound));

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