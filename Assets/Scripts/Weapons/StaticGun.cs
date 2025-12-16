using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GunType { Shotgun, SMG, Pistol }
public class StaticGun : NetworkBehaviour
{
    public static StaticGun Instance;

    [SerializeField] private List<GunTypeDataPair> m_GunTypeDataPairs;
    [SerializeField] private AudioSource m_Source;
    //TODO: create an item database instead of holding this reference
    [SerializeField] private InventoryItem m_NutrientGrenade;
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            Instance = this;
        }
    }
    public void FireClient(GunType gunType, float spread)
    {
        GunData data = m_GunTypeDataPairs.First(g => g.gunType == gunType).gunData;

        for (int i = 0; i < data.bulletsPerShot; i++)
        {
            Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f + Vector2.one * Random.insideUnitCircle * (data.spread + spread));

            FireServer(Owner, gunType, ray.origin, ray.direction, spread);
        }
    }
    [ServerRpc]
    public void FireServer(NetworkConnection conn, GunType gunType, Vector3 origin, Vector3 direction, float spread)
    {
        GunData data = m_GunTypeDataPairs.First(g=>g.gunType == gunType).gunData;

        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, 999, GameManager.Instance.playerIgnoreMask))
        {
            if (hit.transform.CompareTag("Enemy") && hit.transform.TryGetComponent(out IHealth health))
            {
                int finalDamage = Mathf.RoundToInt(data.damage * data.damageFalloff.Evaluate(hit.distance));

                bool died = false;
                health.ChangeHealth(-finalDamage, ref died);

                FireClientCallback(conn, died);
            }
        }

        PlayGunAudio(gunType);
    }
    [TargetRpc]
    private void FireClientCallback(NetworkConnection conn, bool died)
    {
        if (died)
        {
            GameProfile.Instance.currencySystem.AddCurrency(Random.Range(20, 100));
            GameProfile.Instance.inventorySystem.AddItem(m_NutrientGrenade, 1);
        }
    }
    [ObserversRpc(ExcludeOwner = true)]
    public void PlayGunAudio(GunType gunType)
    {
        GunData data = m_GunTypeDataPairs.First(g => g.gunType == gunType).gunData;

        m_Source.pitch = 1 + (Random.value - 0.5f) * 2 * 0.2f;
        m_Source.PlayOneShot(data.fireSound);
    }
}
[System.Serializable]
public class GunTypeDataPair
{
    public GunType gunType;
    public GunData gunData;
}