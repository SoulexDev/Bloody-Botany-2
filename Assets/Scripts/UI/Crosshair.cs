using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public static Crosshair Instance;

    [SerializeField] private Material m_CrosshairMat;
    [SerializeField] private RectTransform m_RectTransform;

    private void Awake()
    {
        Instance = this;
    }
    public void SetCrosshairRadius(float value)
    {
        //visually, the value is r * screen height when the image is fitted to the screen
        //we need the value to be r * screen height no matter the size of the circle
        m_CrosshairMat.SetFloat("_CircleRadius", Screen.height * Mathf.Clamp(value, 0, 0.45f) / m_RectTransform.sizeDelta.y);
    }
    public void SetCrosshairOuterState(bool state)
    {
        m_CrosshairMat.SetFloat("_UseCrosshairOuter", state ? 1f : 0f);
    }
}