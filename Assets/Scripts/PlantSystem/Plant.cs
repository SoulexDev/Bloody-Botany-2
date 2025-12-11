using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrowthStage
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
    private GrowthStage m_GrowthStageBuffer;
    private GrowthStage m_GrowthStage
    {
        get { return m_GrowthStageBuffer; }
        set
        {
            if (value != m_GrowthStageBuffer)
            {
                m_GrowthStageBuffer = value;

                UpdateModel();

                if (m_GrowthStageBuffer == GrowthStage.Mature)
                {
                    isInteractable = true;
                    m_ViewInfo.infoString = $"Harvest {item.name}";
                }
            }
        }
    }
    private Planter m_Planter;
    public override void Awake()
    {
        base.Awake();

        m_ViewInfo.infoString = m_GrowthStage.ToString();
        isInteractable = false;

        UpdateModel();
    }
    public override void OnPickupSuccess()
    {
        base.OnPickupSuccess();
        m_Planter.inUse = false;
    }
    public void Initialize(Planter planter)
    {
        m_Planter = planter;
    }
    private void Update()
    {
        if (m_GrowthStage == GrowthStage.Mature)
            return;

        m_GrowthProgress += Time.deltaTime / GameManager.Instance.difficultySettings.plantGrowthRate;

        if (m_GrowthProgress >= 1)
        {
            m_GrowthProgress -= 1;
            m_GrowthStage++;

            m_ViewInfo.infoString = $"{m_GrowthStage}";
        }

    }
    private void UpdateModel()
    {
        m_PlantModels.ForEach(m=>m.SetActive(false));
        m_PlantModels[(int)m_GrowthStage].SetActive(true);
    }
    public void Feed()
    {
        m_GrowthProgress += 1;
    }
}