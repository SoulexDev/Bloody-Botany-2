using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SmogwalkerState { Chase, Attack, Dead }
public enum SmogwalkerTarget { Player, Shield }
public class Smogwalker : StateMachine<Smogwalker>
{
    public HealthComponent healthComponent;
    public NavMeshAgent agent;
    public Animator anims;
    public Transform visual;

    [SerializeField] private GameObject m_DamageVolume;

    [SerializeField] private AnimationCurve m_ImpactXZCurve;
    [SerializeField] private AnimationCurve m_ImpactYCurve;

    private void Awake()
    {
        stateDictionary.Add(SmogwalkerState.Chase, new SmogwalkerChase());
        stateDictionary.Add(SmogwalkerState.Attack, new SmogwalkerAttack());

        SwitchState(SmogwalkerState.Chase);

        healthComponent.OnHealthDepleted += Die;
        healthComponent.OnHealthLost += DoImpactEffect;
    }
    private void OnDestroy()
    {
        healthComponent.OnHealthDepleted -= Die;
        healthComponent.OnHealthLost -= DoImpactEffect;
    }
    public override void Update()
    {
        base.Update();
    }
    public Vector3 GetNearestTarget(out SmogwalkerTarget targetType)
    {
        Vector3 playerPos = Player.Instance.playerController.transform.position;

        if (!ShieldManager.Instance.GetActiveShieldPosition(out Vector3 shieldPos))
        {
            targetType = SmogwalkerTarget.Player;
            return playerPos;
        }

        if (Vector3.Distance(transform.position, playerPos) < Vector3.Distance(transform.position, shieldPos))
        {
            targetType = SmogwalkerTarget.Player;
            return playerPos;
        }
        else
        {
            targetType = SmogwalkerTarget.Shield;
            return shieldPos;
        }
    }
    private void Die()
    {
        if (Random.value >= 0.5f)
        {
            Player.Instance.currencySystem.AddCurrency(Random.Range(50, 100));
        }
        Destroy(gameObject);
    }
    private void DoImpactEffect()
    {
        StopAllCoroutines();
        StartCoroutine(ImpactAnimation());
    }
    private IEnumerator ImpactAnimation()
    {
        visual.localScale = Vector3.one;

        float timer = 0;

        while (timer < 1)
        {
            float xz = m_ImpactXZCurve.Evaluate(timer);
            float y = m_ImpactYCurve.Evaluate(timer);

            visual.localScale = new Vector3(xz, y, xz);

            timer += Time.deltaTime * 5;
            yield return null;
        }

        visual.localScale = Vector3.one;
    }
    public void SetDamageTriggerActiveState(bool state)
    {
        m_DamageVolume.SetActive(state);
    }
}