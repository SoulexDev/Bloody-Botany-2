//==========================================( Neverway 2025 )=========================================================//
// Author
//  Liz M.
//
// Contributors
//
//
//====================================================================================================================//

using TMPro;
using UnityEngine;

public class UI_ZoneArea : MonoBehaviour
{
    #region========================================( Variables )======================================================//
    /*-----[ Inspector Variables ]------------------------------------------------------------------------------------*/


    /*-----[ External Variables ]-------------------------------------------------------------------------------------*/


    /*-----[ Internal Variables ]-------------------------------------------------------------------------------------*/


    /*-----[ Reference Variables ]------------------------------------------------------------------------------------*/
    public ZoneObject linkedZoneObject;
    public TMP_Text zoneText;
    public Animator animator;


    #endregion


    #region=======================================( Functions )=======================================================//
    /*-----[ Mono Functions ]-----------------------------------------------------------------------------------------*/
    private void Start()
    {
        linkedZoneObject.OnZoneChanged -= OnZoneChanged;
        linkedZoneObject.OnZoneChanged += OnZoneChanged;
    }


    /*-----[ Internal Functions ]-------------------------------------------------------------------------------------*/


    /*-----[ External Functions ]-------------------------------------------------------------------------------------*/
    public void OnZoneChanged()
    {
        zoneText.text = linkedZoneObject.currentZone.name;
        animator.Play("ZoneChanged");
    }


    #endregion
}
