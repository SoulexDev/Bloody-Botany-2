using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct ServerMeleeData
{
    public MeleeWeaponType meleeWeaponType;
    public Vector3 origin;
    public Vector3 direction;
    public float damage;
}
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
        MeleeData data = m_MeleeTypeDataPairs.First(d => d.meleeWeaponType == meleeWeaponType).meleeData;

        Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);

        ServerMeleeData sMeleeData = new ServerMeleeData();
        sMeleeData.meleeWeaponType = meleeWeaponType;

        //TODO: Move into normal melee and call onyl on change
        //TODO: Increase melee speed too
        sMeleeData.damage = PerksManager.Instance.GetPerkValue(PerkType.Damage_Firing, data.damage);
        sMeleeData.origin = ray.origin;
        sMeleeData.direction = ray.direction;

        MeleeServer(Owner, sMeleeData);
    }
    [ServerRpc]
    public void MeleeServer(NetworkConnection conn, ServerMeleeData sMeleeData, Channel channel = Channel.Unreliable)
    {
        MeleeData data = m_MeleeTypeDataPairs.First(d=>d.meleeWeaponType == sMeleeData.meleeWeaponType).meleeData;
        Collider[] cols = Physics.OverlapSphere(sMeleeData.origin + sMeleeData.direction * 1.5f, 2, GameManager.Instance.playerIgnoreMask);

        int sweepTotal = 0;

        foreach (Collider col in cols)
        {
            if (sweepTotal >= data.sweepCount)
                break;

            if (col.CompareTag("Enemy") && col.TryGetComponent(out IHealth health))
            {
                sweepTotal++;

                bool died = false;
                health.ChangeHealth(-Mathf.RoundToInt(sMeleeData.damage), ref died);

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