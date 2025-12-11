using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesPlayOnEnable : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_Particles;

    private void OnEnable()
    {
        m_Particles.Play();
    }
}