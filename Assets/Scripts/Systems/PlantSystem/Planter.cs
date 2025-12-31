using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

public class Planter : NetworkBehaviour
{
    public Transform plantSpawnPoint;

    [AllowMutableSyncType] public SyncVar<bool> inUse = new SyncVar<bool>();

    [ServerRpc(RequireOwnership = false)]
    public void SetInUseState(bool state, Channel channel = Channel.Unreliable)
    {
        inUse.Value = state;
    }
}