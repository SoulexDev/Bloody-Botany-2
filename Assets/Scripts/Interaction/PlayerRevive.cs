using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;

public class PlayerRevive : Interactable
{
    public override void OnInteract()
    {
        base.OnInteract();

        ReviveServer();
    }
    [ServerRpc(RequireOwnership = false)]
    private void ReviveServer(Channel channel = Channel.Unreliable)
    {
        ReviveClient(Owner);
    }
    [TargetRpc]
    private void ReviveClient(NetworkConnection conn, Channel channel = Channel.Unreliable)
    {
        GameProfile.Instance.playerHealth.Revive();
    }
}