using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PowerGenerator : Interactable
{
    [AllowMutableSyncType] public SyncVar<bool> activated = new SyncVar<bool>(false);
    public override void OnInteract()
    {
        base.OnInteract();

        ActivateGenerator();
        isInteractable = false;
    }
    [ServerRpc(RequireOwnership = false)]
    private void ActivateGenerator()
    {
        activated.Value = true;
    }
}