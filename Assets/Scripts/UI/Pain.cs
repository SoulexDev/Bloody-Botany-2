//==========================================( Neverway 2025 )=========================================================//
// Author
//  Liz M.
//
// Contributors
//
//
//====================================================================================================================//

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Pain : MonoBehaviour
{
    #region========================================( Variables )======================================================//
    /*-----[ Inspector Variables ]------------------------------------------------------------------------------------*/
    public float fadeSpeed=1;
    public float startAlpha=1;


    /*-----[ External Variables ]-------------------------------------------------------------------------------------*/


    /*-----[ Internal Variables ]-------------------------------------------------------------------------------------*/
    private bool isInPain; // Used to keep track of if we are currently fading the image in or out (Fadeout: Underground reference?)
    

    /*-----[ Reference Variables ]------------------------------------------------------------------------------------*/
    private Image image;
    [SerializeField] private HealthComponent healthComponent;


    #endregion


    #region=======================================( Functions )=======================================================//
    /*-----[ Mono Functions ]-----------------------------------------------------------------------------------------*/
    private void Start()
    {
        image = CanvasFinder.Instance.painImage;
        healthComponent.OnHealthLost += OnHurt;
    }

    private void OnDestroy()
    {
        healthComponent.OnHealthLost -= OnHurt;
    }


    /*-----[ Internal Functions ]-------------------------------------------------------------------------------------*/

    private IEnumerator FadeInPain()
    {
        isInPain = true;
        DOVirtual.Color(
            new Color(image.color.r, image.color.g, image.color.b, startAlpha), 
            new Color(image.color.r, image.color.g, image.color.b, 0),
            fadeSpeed,
            (value) =>
            {
                image.color = value;
            });
        yield return new WaitForSeconds(0.25f);
        isInPain = false;
    }

    private void OnHurt()
    {
        if (!isInPain)
        {
            StartCoroutine(FadeInPain());
        }
    }


    /*-----[ External Functions ]-------------------------------------------------------------------------------------*/


    #endregion
}