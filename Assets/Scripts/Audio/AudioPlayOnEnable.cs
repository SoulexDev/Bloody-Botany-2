using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayOnEnable : MonoBehaviour
{
    [SerializeField] private AudioSource m_Source;

    private void OnEnable()
    {
        m_Source.Play();
    }
}