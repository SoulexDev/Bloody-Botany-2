using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    public static ShieldManager Instance;

    private Shield m_CurrentSheild;

    private void Awake()
    {
        Instance = this;
    }
    public bool TrySetCurrentShield(Shield shield)
    {
        if (m_CurrentSheild != null)
        {
            if (m_CurrentSheild.shieldState != ShieldState.Active)
            {
                m_CurrentSheild = shield;
                m_CurrentSheild.OnShieldStateChanged += ShieldStateChanged;
                return true;
            }
        }
        else
        {
            m_CurrentSheild = shield;
            m_CurrentSheild.OnShieldStateChanged += ShieldStateChanged;

            return true;
        }

        return false;
    }
    public bool GetActiveShieldPosition(out Vector3 position)
    {
        if (m_CurrentSheild)
        {
            position = m_CurrentSheild.transform.position;
            return true;
        }
        else
        {
            position = Vector3.zero;
            return false;
        }
    }
    private void ShieldStateChanged()
    {
        switch (m_CurrentSheild.shieldState)
        {
            case ShieldState.Inactive:
                m_CurrentSheild.OnShieldStateChanged -= ShieldStateChanged;
                m_CurrentSheild = null;
                break;
            case ShieldState.Active:
                break;
            case ShieldState.Broken:
                m_CurrentSheild.OnShieldStateChanged -= ShieldStateChanged;
                m_CurrentSheild = null;
                break;
            case ShieldState.Locked:
                m_CurrentSheild.OnShieldStateChanged -= ShieldStateChanged;
                m_CurrentSheild = null;
                break;
            default:
                break;
        }
    }
}