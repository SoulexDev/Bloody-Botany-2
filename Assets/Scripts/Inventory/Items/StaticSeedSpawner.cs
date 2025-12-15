using FishNet.Connection;
using FishNet.Object;
using FishNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum SeedType : byte { Shotgun, SMG }
public class StaticSeedSpawner : NetworkBehaviour
{
    public static StaticSeedSpawner Instance;

    [SerializeField] private List<SeedData> m_SeedData;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            Instance = this;
        }
    }
    public void TryThrowClient(SeedType seedType)
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);
        //Check if the player has the seed item and send a throw call to the server
        if (GameProfile.Instance.inventorySystem.HasItem(m_SeedData.First(d=>d.seedType == seedType).seedItem))
        {
            TryThrowServer(seedType, ray.origin, ray.direction, LocalConnection);
        }
    }
    [ServerRpc]
    private void TryThrowServer(SeedType seedType, Vector3 origin, Vector3 direction, NetworkConnection conn)
    {
        //Try to throw on the server
        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, 999, GameManager.Instance.seedThrowMask))
        {
            if (hit.transform.TryGetComponent(out Planter planter) && !planter.inUse.Value)
            {
                planter.inUse.Value = true;
                NetworkObject nob = Instantiate(m_SeedData.First(d=>d.seedType == seedType).spawnPlant, 
                    planter.plantSpawnPoint.position, Quaternion.identity);

                if (nob.TryGetComponent(out Plant plant))
                {
                    plant.Initialize(planter);
                }
                else
                    Debug.LogError("Seed plant NOB does not contain 'Plant' Component. Fix this tard fuck");

                InstanceFinder.ServerManager.Spawn(nob);

                SeedPlantCallback(conn, seedType, true);
            }
            else
                SeedPlantCallback(conn, seedType, false);
        }
        else
            SeedPlantCallback(conn, seedType, false);
    }
    //Confirm on the client that a seed was planted and remove the seed item
    //TODO: Handle either throw animation or no throw animation
    [TargetRpc]
    private void SeedPlantCallback(NetworkConnection conn, SeedType seedType, bool value)
    {
        if (!GameProfile.Instance.inventorySystem.RemoveItem(m_SeedData.First(d => d.seedType == seedType).seedItem))
        {
            Debug.LogError("Failed to plant seed after confirmation. Client desync");
        }
    }
}
[System.Serializable]
public class SeedData
{
    public SeedType seedType;
    public NetworkObject spawnPlant;
    public InventoryItem seedItem;
}