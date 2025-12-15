using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerVolume : MonoBehaviour
{
    [SerializeField] private Mesh m_VolumeMesh;
    [SerializeField] private Color m_VolumeColor;
    [SerializeField] private BoxCollider m_BoxVolume;
    [SerializeField] private string m_TriggerValidTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_TriggerValidTag))
        {
            OnValidEnter(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(m_TriggerValidTag))
        {
            OnValidExit(other);
        }
    }
    public virtual void OnValidEnter(Collider other)
    {

    }
    public virtual void OnValidExit(Collider other)
    {

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = m_VolumeColor;
        Vector3 position = transform.position + transform.TransformDirection(m_BoxVolume.center);
        Vector3 scale = Vector3.Scale(m_BoxVolume.size, transform.localScale);
        Gizmos.DrawWireMesh(m_VolumeMesh, position, transform.rotation, scale);

        Color col = m_VolumeColor;
        col.a = 0.25f;
        Gizmos.color = col;
        Gizmos.DrawMesh(m_VolumeMesh, position, transform.rotation, scale);
    }
}