using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(AudioLowPassFilter))]
public class AdvancedAudioSource : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioLowPassFilter m_LowPassFilter;
    public AudioClip[] clips;
    public bool isGlobal;
    public bool useOcclusion = true;

    [HideInInspector] public Vector2 pitchVariation = Vector2.zero;

    private void Awake()
    {
        if (isGlobal)
            m_AudioSource.spatialBlend = 0;
    }
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position, transform.up);

        float finalCutoffFrequency = 22000;
        if (isGlobal)
        {
            finalCutoffFrequency = useOcclusion && EarAttenuator.Instance.isOccluded ? 1000 : 22000;
            m_AudioSource.panStereo = EarAttenuator.Instance.leftRightAttenuation;
        }
        else
        {
            finalCutoffFrequency = useOcclusion && IsOccluded() ? 1000 : 22000;
        }

        m_LowPassFilter.cutoffFrequency = Mathf.Lerp(m_LowPassFilter.cutoffFrequency, finalCutoffFrequency, Time.deltaTime * 5);
        //m_LowPassFilter.cutoffFrequency = finalCutoffFrequency;
    }
    public bool IsOccluded()
    {
        for (int i = 0; i < 4; i++)
        {
            //TODO: use audio mask
            Vector3 pos = transform.position + Quaternion.AngleAxis(i * 90, transform.forward) * transform.up;
            if (!Physics.Linecast(pos, Camera.main.transform.position, out RaycastHit hit))
                return false;
        }
        return true;
    }
    public void PlayOneShot()
    {
        m_AudioSource.pitch = 1 + Random.Range(pitchVariation.x, pitchVariation.y);
        m_AudioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
    public void Play()
    {
        m_AudioSource.pitch = 1 + Random.Range(pitchVariation.x, pitchVariation.y);
        m_AudioSource.clip = clips[Random.Range(0, clips.Length)];
        m_AudioSource.Play();
    }
    private void OnDrawGizmos()
    {
        bool isOccluded = true;
        for (int i = 0; i < 4; i++)
        {
            Vector3 pos = transform.position + Quaternion.AngleAxis(i * 90, transform.forward) * transform.up;
            if (!Physics.Linecast(pos, Camera.main.transform.position, out RaycastHit hit))
            {
                isOccluded = false;
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(hit.point, 0.1f);
            }
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pos, Camera.main.transform.position);
        }
        if (isOccluded)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.2f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.2f);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AdvancedAudioSource))]
public class AdvancedAudioSourceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AdvancedAudioSource source = (AdvancedAudioSource)target;

        EditorGUI.BeginChangeCheck();

        //Show Pitch Variation
        EditorGUILayout.LabelField("Pitch Variation");
        EditorGUILayout.BeginHorizontal();

        source.pitchVariation.x = EditorGUILayout.FloatField(source.pitchVariation.x);
        EditorGUILayout.MinMaxSlider(ref source.pitchVariation.x, ref source.pitchVariation.y, -1, 1);
        source.pitchVariation.y = EditorGUILayout.FloatField(source.pitchVariation.y);

        EditorGUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endif