using FishNet.Connection;
using FishNet.Object;

public class PlayerRevive : Interactable
{
    public override void OnInteract()
    {
        base.OnInteract();

        ReviveServer();
    }
    [ServerRpc(RequireOwnership = false)]
    private void ReviveServer()
    {
        ReviveClient(Owner);
    }
    [TargetRpc]
    private void ReviveClient(NetworkConnection conn)
    {
        GameProfile.Instance.playerHealth.Revive();
    }
}