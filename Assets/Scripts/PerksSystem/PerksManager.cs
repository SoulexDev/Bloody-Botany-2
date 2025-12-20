using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerksManager : NetworkBehaviour
{
    public static PerksManager Instance;

    private List<PerkSlot> m_PerkSlots => CanvasFinder.Instance.perkSlots;
    private float m_PerkLossTimer;

    public delegate void PerksChanged();
    public static event PerksChanged OnPerksChanged;

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

        if (IsOwner)
            OnPerksChanged?.Invoke();
    }
    private void RemoveRandomPerk()
    {
        List<PerkSlot> validSlots = m_PerkSlots.FindAll(s=>s.perk != null);

        if (validSlots.Count == 0)
            return;

        PerkSlot slot = validSlots[Random.Range(0, validSlots.Count)];
        slot.perkCount--;

        if (IsOwner)
            OnPerksChanged?.Invoke();
    }
    //Only use when perks change
    public float GetPerkValue(PerkType perkType, float baseNum)
    {
        //if (!m_PerkSlots.Any(s=>s.perkCount > 0 && s.perk.perkType == perkType))
        //    return baseNum;

        PerkSlot slot = m_PerkSlots.FirstOrDefault(s=>s.perkCount > 0 && s.perk.perkType == perkType);

        if (!slot)
            return baseNum;

        return MathFunctions.GetFromFunction(baseNum, slot.perk.coefficient, slot.perkCount, slot.perk.mathFunction);
    }
}