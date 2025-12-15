using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class Planter : NetworkBehaviour
{
    public Transform plantSpawnPoint;

    //TODO: make only on server and shint.. wait no
    [AllowMutableSyncType] public SyncVar<bool> inUse = new SyncVar<bool>();

    [ServerRpc(RequireOwnership = false)]
    public void SetInUseState(bool state)
    {
        inUse.Value = state;
    }
}