using FishNet;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ThrowableType { NutrientGrenade }
public class StaticThrowableSpawner : NetworkBehaviour
{
    public static StaticThrowableSpawner Instance;

    [SerializeField] private List<ThrowableData> m_ThrowableData;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            Instance = this;
        }
    }
    public void Throw(ThrowableType throwableType)
    {
        ThrowableData data = m_ThrowableData.First(t=>t.throwableType == throwableType);

        if (GameProfile.Instance.inventorySystem.RemoveItem(data.throwableItem))
        {
            Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);

            ThrowServer(throwableType, ray.origin, ray.direction);
        }
    }
    [ServerRpc]
    private void ThrowServer(ThrowableType throwableType, Vector3 origin, Vector3 direction)
    {
        ThrowableData data = m_ThrowableData.First(t => t.throwableType == throwableType);

        NetworkObject nob = InstanceFinder.NetworkManager.GetPooledInstantiated(data.throwable, origin, Quaternion.identity, true);

        InstanceFinder.ServerManager.Spawn(nob);

        if (nob.TryGetComponent(out Throwable throwable))
        {
            throwable.Throw(direction);
        }
        else
        {
            Debug.LogError("Throwable does not have throwable component!!");
        }
    }
}
[System.Serializable]
public class ThrowableData
{
    public ThrowableType throwableType;
    public InventoryItem throwableItem;
    public NetworkObject throwable;
}