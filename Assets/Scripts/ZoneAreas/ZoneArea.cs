//==========================================( Neverway 2025 )=========================================================//
// Author
//  Liz M.
//
// Contributors
//
//
//====================================================================================================================//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneArea : MonoBehaviour
{
    #region========================================( Variables )======================================================//
    /*-----[ Inspector Variables ]------------------------------------------------------------------------------------*/


    /*-----[ External Variables ]-------------------------------------------------------------------------------------*/
    [Tooltip("True when a player has occupied this zone at least once, used for determining if this zone is can be activated")]
    public bool hasBeenVisited = false;
    [Tooltip("True when the zone is occupied by a player, or if an adjacent zone is occupied and this zone has already been visited")]
    public bool zoneActive = false;


    /*-----[ Internal Variables ]-------------------------------------------------------------------------------------*/


    /*-----[ Reference Variables ]------------------------------------------------------------------------------------*/
    public List<ZoneArea> adjacentZones = new List<ZoneArea>();
    public List<ZoneObject> visitors = new List<ZoneObject>();
    public Action OnZoneActivated, OnZoneDeactivated;


    #endregion


    #region=======================================( Functions )=======================================================//
    /*-----[ Mono Functions ]-----------------------------------------------------------------------------------------*/


    /*-----[ Internal Functions ]-------------------------------------------------------------------------------------*/
    private void CheckAllZoneStates()
    {
        CheckZoneState();
        foreach (ZoneArea adjacentZone in adjacentZones)
        {
            adjacentZone.CheckZoneState();
        }
    }
    
    private void CheckZoneState()
    {
        // Don't activate unvisited zones
        if (hasBeenVisited is false) return;
        
        // Zone is now occupied
        if (visitors.Count == 1)
        {
            zoneActive = true;
            OnZoneActivated?.Invoke();
        }
        // Zone was already occupied
        else if (visitors.Count > 1)
        {
            
        }
        // Zone is unoccupied
        else if (visitors.Count == 0)
        {
            zoneActive = false;
            OnZoneDeactivated?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var adjacentZone in adjacentZones)
        {
            if (adjacentZone is null) continue;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(adjacentZone.transform.position, transform.position);
        }
    }
    private void OnDrawGizmosSelected()
    {
        foreach (var adjacentZone in adjacentZones)
        {
            if (adjacentZone is null) continue;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(adjacentZone.transform.position, transform.position+(Vector3.up*0.25f));
        }
    }


    /*-----[ External Functions ]-------------------------------------------------------------------------------------*/
    public void AddVisitor(ZoneObject zoneObject)
    {
        if (visitors == null)
        {
            print(gameObject.name + " has no visitor set");
            visitors = new List<ZoneObject>();
        }
        visitors.Add(zoneObject);
        hasBeenVisited = true;
        CheckAllZoneStates();
    }

    public void RemoveVisitor(ZoneObject zoneObject)
    {
        if (visitors == null)
        {
            print(gameObject.name + " has no visitor set");
            visitors = new List<ZoneObject>();
        }
        visitors.Remove(zoneObject);
        CheckAllZoneStates();
    }


    #endregion
}
