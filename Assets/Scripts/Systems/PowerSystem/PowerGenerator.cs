using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PowerGenerator : Interactable
{
    [AllowMutableSyncType] public SyncVar<bool> activated = new SyncVar<bool>(false);
    public override void Awake()
    {
        base.Awake();

        m_ViewInfo.activeInfoString = "Activate Power";
    }
    public override void OnInteract()
    {
        base.OnInteract();

        ActivateGenerator();
        isInteractable = false;

        m_ViewInfo.activeInfoString = "Power Active";
    }
    [ServerRpc(RequireOwnership = false)]
    private void ActivateGenerator()
    {
        activated.Value = true;
    }
}