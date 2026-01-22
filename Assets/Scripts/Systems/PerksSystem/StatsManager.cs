using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsManager : NetworkBehaviour
{
    public static StatsManager Instance;

    private List<PerkSlot> m_PerkSlots => CanvasFinder.Instance.perkSlots;
    private float m_PerkLossTimer;

    //public delegate void PerksChanged();
    //public static event PerksChanged OnPerksChanged;

    public float healthHealingMult = 1;
    public float moveInteractionMult = 1;
    public float accuracyMagMult = 1;
    public float damageFiringMult = 1;
    public float lungCapacityMult = 1;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if (GameProfile.Instance && GameProfile.Instance.playerHealth.dead.Value)
        {
            m_PerkLossTimer += Time.deltaTime / GameManager.Instance.difficultySettings.perkLossRate;

            if (m_PerkLossTimer >= 1)
            {
                m_PerkLossTimer -= 1;
                RemoveRandomPerk();
            }
        }
        else
            m_PerkLossTimer = 0;
    }
    public void AddPerk(Perk perk)
    {
        PerkSlot slot = m_PerkSlots.FirstOrDefault(s=>s.perk == perk);

        if (!slot)
        {
            slot = m_PerkSlots.FirstOrDefault(s=>s.perk == null);
        }

        slot.perk = perk;
        slot.perkCount++;

        UpdateStats(perk.perkType);

        //if (IsOwner)
        //    OnPerksChanged?.Invoke();
    }
    private void RemoveRandomPerk()
    {
        List<PerkSlot> validSlots = m_PerkSlots.FindAll(s=>s.perk != null);

        if (validSlots.Count == 0)
            return;

        PerkSlot slot = validSlots[Random.Range(0, validSlots.Count)];
        PerkType perkType = slot.perk.perkType;
        slot.perkCount--;

        UpdateStats(perkType);

        //if (IsOwner)
        //    OnPerksChanged?.Invoke();
    }
    private void UpdateStats(PerkType perkType)
    {
        PerkSlot slot = m_PerkSlots.FirstOrDefault(s => s.perkCount > 0 && s.perk.perkType == perkType);

        if (!slot)
            return;

        float value = MathFunctions.GetFromFunction(1, slot.perk.coefficient, slot.perkCount, slot.perk.mathFunction);

        switch (perkType)
        {
            case PerkType.MaxHealth_Healing:
                healthHealingMult = value;
                break;
            case PerkType.MoveSpeed_InteractionSpeed:
                moveInteractionMult = value;
                break;
            case PerkType.Accuracy_Magsize:
                accuracyMagMult = value;
                break;
            case PerkType.Damage_FiringSpeed:
                damageFiringMult = value;
                break;
            case PerkType.LungCapacity:
                lungCapacityMult = value;
                break;
            default:
                break;
        }
    }
}