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

public class HealthEffects : MonoBehaviour
{
    #region========================================( Variables )======================================================//
    /*-----[ Inspector Variables ]------------------------------------------------------------------------------------*/
    //public float fadeSpeed=1;
    //public float startAlpha=1;
    //public Color damageColor, reviveColor;
    

    /*-----[ External Variables ]-------------------------------------------------------------------------------------*/


    /*-----[ Internal Variables ]-------------------------------------------------------------------------------------*/
    //private bool isInPain; // Used to keep track of if we are currently fading the image in or out (Fadeout: Underground reference?)
    //private bool wasDead;
    

    /*-----[ Reference Variables ]------------------------------------------------------------------------------------*/
    private Image image;
    //private Tweener tweener;
    [SerializeField] private HealthComponent healthComponent;


    #endregion


    #region=======================================( Functions )=======================================================//
    /*-----[ Mono Functions ]-----------------------------------------------------------------------------------------*/
    private void Start()
    {
        image = CanvasFinder.Instance.painImage;
        //healthComponent.OnHealthLost += OnHurt;
        //healthComponent.OnHealthDepleted += OnDown;
        healthComponent.OnHealthChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        //healthComponent.OnHealthLost -= OnHurt;
        //healthComponent.OnHealthDepleted -= OnDown;
        healthComponent.OnHealthChanged -= OnHealthChanged;
    }


    /*-----[ Internal Functions ]-------------------------------------------------------------------------------------*/

    //private IEnumerator FadeInColor(Color color)
    //{
    //    isInPain = true;
    //    tweener = DOVirtual.Color(
    //        new Color(color.r, color.g, color.b, startAlpha), 
    //        new Color(color.r, color.g, color.b, 0),
    //        fadeSpeed,
    //        (value) => { image.color = value; });
    //    yield return new WaitForSeconds(0.25f);
    //    isInPain = false;
    //}
    //private IEnumerator FadeInDeath()
    //{
    //    isInPain = true;
    //    tweener = DOVirtual.Color(
    //        new Color(damageColor.r, damageColor.g, damageColor.b, 0), 
    //        new Color(damageColor.r, damageColor.g, damageColor.b, startAlpha*2),
    //        fadeSpeed,
    //        (value) => { image.color = value; });
    //    yield return new WaitForSeconds(0.25f);
    //    isInPain = false;
    //    wasDead = true;
    //}

    //private void OnHurt()
    //{
    //    return;
    //    if (!isInPain && !wasDead)
    //    {
    //        StartCoroutine(FadeInColor(damageColor));
    //    }
    //}

    //private void OnDown()
    //{
    //    StopAllCoroutines();
    //    tweener?.Kill();
    //    StartCoroutine(FadeInDeath());
    //}
    
    private void OnHealthChanged()
    {
        image.color = Color.white * (1f - (float)healthComponent.health / healthComponent.maxHealth);
        //if (healthComponent.health <= 0) return;
        //if (!wasDead) return;
        //wasDead = false;
        //StartCoroutine(FadeInColor(reviveColor));
    }


    /*-----[ External Functions ]-------------------------------------------------------------------------------------*/


    #endregion
}