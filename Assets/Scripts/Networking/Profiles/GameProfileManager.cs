using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine;

public class GameProfileManager : NetworkBehaviour
{
    public static GameProfileManager Instance;
    public readonly SyncList<GameProfile> gameProfiles = new SyncList<GameProfile>();

    private void Awake()
    {
        Instance = this;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        gameProfiles.OnChange += GameProfiles_OnChange;
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        gameProfiles.OnChange -= GameProfiles_OnChange;
    }
    private void GameProfiles_OnChange(SyncListOperation op, int index, GameProfile oldItem, GameProfile newItem, bool asServer)
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
    public GameProfile GetGameProfileByConnection(NetworkConnection conn)
    {
        gameProfiles.ToList().ForEach(g=>print($"Owner: {g.Owner}, Connection: {conn}"));
        return gameProfiles.First(g=>g.Owner == conn);
    }
}