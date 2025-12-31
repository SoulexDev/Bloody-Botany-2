using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrowthStage : byte
{
    Seed = 0,
    Sprout = 1,
    Flowering = 2,
    Mature = 3
}
public class Plant : ItemPickup
{
    [SerializeField] private List<GameObject> m_PlantModels;

    private float m_GrowthProgress = 0;
    [AllowMutableSyncType] private SyncVar<GrowthStage> m_GrowthStage = new SyncVar<GrowthStage>();
    private Planter m_Planter;
    public override void Awake()
    {
        base.Awake();

        m_ViewInfo.infoString = m_GrowthStage.Value.ToString();
        isInteractable = false;

        m_GrowthStage.OnChange += GrowthStage_OnChange;
    }
    private void OnDestroy()
    {
        m_GrowthStage.OnChange -= GrowthStage_OnChange;
    }
    public void Initialize(Planter planter)
    {
        m_Planter = planter;
    }
    private void GrowthStage_OnChange(GrowthStage prev, GrowthStage next, bool asServer)
    {
        UpdateModel(next);

        if (next == GrowthStage.Mature)
        {
            isInteractable = true;
            m_ViewInfo.infoString = $"Harvest {item.name}";
        }
        else
        {
            m_ViewInfo.infoString = m_GrowthStage.Value.ToString();
        }
    }
    private void Update()
    {
        if (!IsServerInitialized)
            return;

        UpdateServer();
    }
    [Server]
    private void UpdateServer()
    {
        if (m_GrowthStage.Value == GrowthStage.Mature)
            return;

        m_GrowthProgress += Time.deltaTime / GameManager.Instance.difficultySettings.plantGrowthRate;

        if (m_GrowthProgress >= 1)
        {
            m_GrowthProgress -= 1;
            m_GrowthStage.Value++;
        }
    }
    public override void OnPickup()
    {
        base.OnPickup();
        OnPickupServer();
    }
    [ServerRpc(RequireOwnership = false)]
    private void OnPickupServer(Channel channel = Channel.Unreliable)
    {
        m_Planter.SetInUseState(false);
    }
    private void UpdateModel(GrowthStage stage)
    {
        m_PlantModels.ForEach(m=>m.SetActive(false));
        m_PlantModels[(int)stage].SetActive(true);
    }
    public void Feed()
    {
        m_GrowthProgress += 1;
    }
}