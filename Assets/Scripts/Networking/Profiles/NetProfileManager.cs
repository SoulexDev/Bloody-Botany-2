using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine;

public class NetProfileManager : NetworkBehaviour
{
    public static NetProfileManager Instance;
    public readonly SyncList<NetProfile> netProfiles = new SyncList<NetProfile>();

    private void Awake()
    {
        Instance = this;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        netProfiles.OnChange += NetProfiles_OnChange;
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        netProfiles.OnChange -= NetProfiles_OnChange;
    }
    private void NetProfiles_OnChange(SyncListOperation op, int index, NetProfile oldItem, NetProfile newItem, bool asServer)
    {
        switch (op)
        {
            case SyncListOperation.Add:
                break;
            case SyncListOperation.Insert:
                break;
            case SyncListOperation.Set:
                break;
            case SyncListOperation.RemoveAt:
                break;
            case SyncListOperation.Clear:
                break;
            case SyncListOperation.Complete:
                break;
            default:
                break;
        }
    }
    public NetProfile GetNetProfileByConnection(NetworkConnection conn)
    {
        //netProfiles.ToList().ForEach(g => print($"Owner: {g.Owner}, Connection: {conn}"));
        return netProfiles.FirstOrDefault(g => g.Owner == conn);
    }
}