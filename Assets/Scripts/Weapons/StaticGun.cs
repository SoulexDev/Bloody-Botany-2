using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GunType : byte { Shotgun, SMG, Pistol, Revolver }
public struct ServerGunData
{
    public GunType gunType;
    public Vector3 origin;
    public Vector3 direction;
    public float damage;
}
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
    public void FireClient(GunType gunType, float spread, float damage)
    {
        //Get gun data
        GunData data = m_GunTypeDataPairs.First(g => g.gunType == gunType).gunData;

        //Create server data
        ServerGunData sGunData = new ServerGunData();
        sGunData.gunType = gunType;
        sGunData.damage = damage;

        for (int i = 0; i < data.bulletsPerShot; i++)
        {
            //Create rays and assign values to server data before sending off to server
            Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f + Vector2.one * Random.insideUnitCircle * spread);
            sGunData.origin = ray.origin;
            sGunData.direction = ray.direction;

            FireServer(Owner, sGunData);
        }
    }
    [ServerRpc]
    public void FireServer(NetworkConnection conn, ServerGunData sGunData, Channel channel = Channel.Unreliable)
    {
        //Reconstruct gun data on server
        GunData data = m_GunTypeDataPairs.First(g=>g.gunType == sGunData.gunType).gunData;

        if (Physics.Raycast(sGunData.origin, sGunData.direction, out RaycastHit hit, 999, GameManager.Instance.playerIgnoreMask))
        {
            if (hit.transform.CompareTag("Enemy") && hit.transform.TryGetComponent(out IHealth health))
            {
                //Get final damage by evaluating falloff
                int finalDamage = Mathf.RoundToInt(sGunData.damage * data.damageFalloff.Evaluate(hit.distance));

                bool died = false;
                health.ChangeHealth(-finalDamage, ref died);

                FireClientCallback(conn, died);
            }
        }

        PlayGunAudio(sGunData.gunType);
    }
    [TargetRpc]
    private void FireClientCallback(NetworkConnection conn, bool died, Channel channel = Channel.Unreliable)
    {
        if (died)
        {
            //TODO: Move on kill logic elsewhere
            GameProfile.Instance.currencySystem.AddCurrency(Random.Range(
            GameManager.Instance.difficultySettings.onKillPaymentLowerBound,
            GameManager.Instance.difficultySettings.onKillPaymentUpperBound));

            GameProfile.Instance.inventorySystem.AddItem(m_NutrientGrenade, 1);
        }
    }
    [ObserversRpc(ExcludeOwner = true)]
    public void PlayGunAudio(GunType gunType, Channel channel = Channel.Unreliable)
    {
        GunData data = m_GunTypeDataPairs.First(g => g.gunType == gunType).gunData;

        m_Source.pitch = 1 + (Random.value - 0.5f) * 2 * 0.2f;
        m_Source.PlayOneShot(data.fireSound);
    }

    
    //public static void WriteServerGunData(this Writer writer, ServerGunData value)
    //{
    //    writer.WriteByte((byte)value.gunType);
    //}
}
[System.Serializable]
public class GunTypeDataPair
{
    public GunType gunType;
    public GunData gunData;
}