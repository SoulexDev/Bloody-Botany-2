using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    private Shield m_CurrentSheild;

    private void Update()
    {
        if (m_CurrentSheild.shieldState == ShieldState.Active)
        {

        }
    }
    public bool TrySetCurrentShield(Shield shield)
    {
        if (m_CurrentSheild != null)
        {
            if (m_CurrentSheild.shieldState != ShieldState.Active)
            {
                m_CurrentSheild.OnShieldStateChanged -= ShieldStateChanged;
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
    private void ShieldStateChanged()
    {

    }
}