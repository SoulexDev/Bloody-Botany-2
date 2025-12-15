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
using UnityEngine.Splines;

public class TriggerZipline : Interactable
{
    #region========================================( Variables )======================================================//
    /*-----[ Inspector Variables ]------------------------------------------------------------------------------------*/


    /*-----[ External Variables ]-------------------------------------------------------------------------------------*/


    /*-----[ Internal Variables ]-------------------------------------------------------------------------------------*/


    /*-----[ Reference Variables ]------------------------------------------------------------------------------------*/
    public SplineContainer spline;
    public GameObject targetObject;
    [SerializeField] private bool ziplineActive;
    [SerializeField] private float currentTime = 0f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector3 ziplineOffset;
    [SerializeField] private List<GameObject> playersInTrigger = new List<GameObject>();
    [SerializeField] private Transform dismountPoint;


    #endregion


    #region=======================================( Functions )======================================================= //

    /*-----[ Mono Functions ]-----------------------------------------------------------------------------------------*/
    public void Update()
    {
        if (spline == null || !ziplineActive || targetObject == null) return;
        
        currentTime += (speed / spline.CalculateLength()) * Time.deltaTime;
        currentTime = Mathf.Clamp01(currentTime);
        
        Vector3 currentPosition = spline.EvaluatePosition(currentTime);
        var currentTangent = spline.EvaluateTangent(currentTime);
        
        targetObject.transform.position = currentPosition + ziplineOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playersInTrigger.Contains(other.gameObject))
        {
            playersInTrigger.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (playersInTrigger.Contains(other.gameObject))
        {
            playersInTrigger.Remove(other.gameObject);
        }
    }


    /*-----[ Internal Functions ]-------------------------------------------------------------------------------------*/
    public override void OnInteract()
    {
        base.OnInteract();
        StartCoroutine(StartZipline());
    }

    private IEnumerator StartZipline()
    {
        if (playersInTrigger.Count == 0) yield break;
        
        var playerRigidbody = targetObject.GetComponent<Rigidbody>();
        if (playerRigidbody != null) { playerRigidbody.isKinematic = true; }
        targetObject = playersInTrigger[0];
        ziplineActive = true;
        currentTime = 0f;
        
        while (currentTime < 1)
        {
            yield return null;
        }
        
        if (playerRigidbody != null) { playerRigidbody.isKinematic = false; }
        ziplineActive = false;
        targetObject.transform.position = dismountPoint.position;
        targetObject = null;
    }


    /*-----[ External Functions ]-------------------------------------------------------------------------------------*/


    #endregion
}
