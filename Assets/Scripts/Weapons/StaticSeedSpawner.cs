using FishNet.Connection;
using FishNet.Object;
using FishNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FishNet.Transporting;

public enum SeedType : byte { Shotgun, SMG, Pistol, Revolver }
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
    public void TryThrowClient(SeedType seedType, int slotIndex)
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);
        //Check if the player has the seed item and send a throw call to the server
        if (GameProfile.Instance.inventorySystem.HasItem(m_SeedData.First(d=>d.seedType == seedType).seedItem))
        {
            TryThrowServer(seedType, ray.origin, ray.direction, slotIndex, LocalConnection);
        }
    }
    [ServerRpc]
    private void TryThrowServer(SeedType seedType, Vector3 origin, Vector3 direction, int slotIndex, NetworkConnection conn, Channel channel = Channel.Unreliable)
    {
        //Try to throw on the server
        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, 999, GameManager.seedThrowMask))
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

                SeedPlantCallback(conn, seedType, slotIndex);
            }
        }
    }
    //Confirm on the client that a seed was planted and remove the seed item
    //TODO: Handle either throw animation or no throw animation
    [TargetRpc]
    private void SeedPlantCallback(NetworkConnection conn, SeedType seedType, int slotIndex, Channel channel = Channel.Unreliable)
    {
        if (!GameProfile.Instance.inventorySystem.RemoveItemFromSlot(GameProfile.Instance.inventorySystem.GetSlotFromIndex(slotIndex)))
        {
            Debug.LogError("Failed to plant seed after confirmation. Client desync");
        }
        else
            AmmoText.Instance.UpdateValue(GameProfile.Instance.inventorySystem.GetSlotFromIndex(slotIndex).itemCount);
    }
}
[System.Serializable]
public class SeedData
{
    public SeedType seedType;
    public NetworkObject spawnPlant;
    public InventoryItem seedItem;
}