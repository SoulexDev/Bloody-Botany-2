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
    [SerializeField] private Transform dismountPoint;


    #endregion


    #region=======================================( Functions )======================================================= //

    /*-----[ Mono Functions ]-----------------------------------------------------------------------------------------*/
    public void Update()
    {
        if (spline == null || !ziplineActive || targetObject == null) return;
    }


    /*-----[ Internal Functions ]-------------------------------------------------------------------------------------*/
    public override void OnInteract()
    {
        base.OnInteract();
        if (ziplineActive) return;
        StartCoroutine(StartZipline());
    }

    private IEnumerator StartZipline()
    {
        targetObject = GameProfile.Instance.playerController.gameObject;
        print("ZIPLINE: " + targetObject.name);
        ziplineActive = true;
        currentTime = 0f;
        
        while (currentTime < 1)
        {
            currentTime += (speed / spline.CalculateLength()) * Time.deltaTime;
            currentTime = Mathf.Clamp01(currentTime);
        
            Vector3 currentPosition = spline.EvaluatePosition(currentTime);
            var currentTangent = spline.EvaluateTangent(currentTime);
        
            targetObject.transform.position = currentPosition + ziplineOffset;
            yield return null;
        }
        
        ziplineActive = false;
        targetObject.transform.position = dismountPoint.position;
        targetObject = null;
    }


    /*-----[ External Functions ]-------------------------------------------------------------------------------------*/


    #endregion
}
