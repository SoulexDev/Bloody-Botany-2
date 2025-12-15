using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTrigger : TriggerVolume
{
    [SerializeField] private List<SpawnZone> m_SpawnZones = new List<SpawnZone>();
    public override void OnValidEnter(Collider other)
    {
        WaveManager.Instance.spawnZones.AddRange(m_SpawnZones);
    }
    public override void OnValidExit(Collider other)
    {
        for (int i = 0; i < m_SpawnZones.Count; i++)
        {
            WaveManager.Instance.spawnZones.Remove(m_SpawnZones[i]);
        }
    }
}