using System.Collections;
using UnityEngine;

public class TestDummy : MonoBehaviour, IHealth
{
    [SerializeField] private AnimationCurve m_XZScaleCurve;
    [SerializeField] private AnimationCurve m_YScaleCurve;
    public void ChangeHealth(int amount, ref bool died)
    {
        StopAllCoroutines();
        StartCoroutine(ImpactAnimation());
    }
    private IEnumerator ImpactAnimation()
    {
        transform.localScale = Vector3.one;

        float timer = 0;

        while (timer < 1)
        {
            float xz = m_XZScaleCurve.Evaluate(timer);
            float y = m_YScaleCurve.Evaluate(timer);

            transform.localScale = new Vector3(xz, y, xz);

            timer += Time.deltaTime * 5;
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}