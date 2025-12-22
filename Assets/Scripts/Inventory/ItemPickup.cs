using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;

public class ItemPickup : Interactable
{
    public InventoryItem item;
    [AllowMutableSyncType] public SyncVar<int> itemCount = new SyncVar<int>(1);

    public override void Awake()
    {
        base.Awake();

        m_ViewInfo.infoString = $"Pick up {item.name}";
    }
    public override void OnInteract()
    {
        base.OnInteract();

        int returnAmount = GameProfile.Instance.inventorySystem.AddItem(item, itemCount.Value);

        if (returnAmount == 0)
        {
            OnPickup();
            RemoveObject();
        }
        else
        {
            SetItemCount(returnAmount);
        }
    }
    public virtual void OnPickup()
    {

    }
    [ServerRpc(RequireOwnership = false)]
    private void SetItemCount(int itemCount, Channel channel = Channel.Unreliable)
    {
        this.itemCount.Value = itemCount;
    }
    [ServerRpc(RequireOwnership = false)]
    private void RemoveObject(Channel channel = Channel.Unreliable)
    {
        Despawn(DespawnType.Destroy);
    }
}