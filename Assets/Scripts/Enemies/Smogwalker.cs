using FishNet;
using FishNet.Object;
using System.Collections;
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

    [SerializeField] private EnemyDamageVolume m_DamageVolume;
    [SerializeField] private AnimationCurve m_ImpactXZCurve;
    [SerializeField] private AnimationCurve m_ImpactYCurve;
    [SerializeField] private Renderer m_Renderer;
    private Material m_RendMat;

    [HideInInspector] public float animsSpeed;

    private void Awake()
    {
        Material[] mats = m_Renderer.sharedMaterials;
        mats[0] = new Material(mats[0]);

        m_RendMat = mats[0];
        m_Renderer.sharedMaterials = mats;

        m_RendMat.SetFloat("_Flash", 0);
    }
    private void OnDestroy()
    {
        if (!IsServerInitialized)
            return;

        healthComponent.OnHealthDepleted -= Die;
        healthComponent.OnHealthLost -= DoImpactEffect;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();

        stateDictionary.Add(SmogwalkerState.Chase, new SmogwalkerChase());
        stateDictionary.Add(SmogwalkerState.Attack, new SmogwalkerAttack());

        SwitchState(SmogwalkerState.Chase);

        healthComponent.OnHealthDepleted += Die;
        healthComponent.OnHealthLost += DoImpactEffect;

        float buff = Random.Range(0, 3) switch
        {
            0 => 0.5f,
            1 => 1f,
            2 => 2f,
            _ => 0.5f
        };

        healthComponent.health = GameManager.Instance.GetEnemyHealth(healthComponent.maxHealth, buff);

        float speed = GameManager.Instance.GetEnemySpeed(agent.speed, buff);
        anims.speed = speed / agent.speed;
        agent.speed = speed;

        animsSpeed = anims.speed;

        m_DamageVolume.damage = GameManager.Instance.GetEnemyDamage(m_DamageVolume.damage, buff);
    }
    [Server]
    public Vector3 GetNearestTarget(out SmogwalkerTarget targetType)
    {
        float furthest = float.PositiveInfinity;
        int closestIndex = -1;
        for (int i = 0; i < GameProfileManager.Instance.gameProfiles.Count; i++)
        {
            GameProfile profile = GameProfileManager.Instance.gameProfiles[i];

            if (profile.playerHealth.dead.Value)
                continue;

            float distance = Vector3.Distance(transform.position, profile.playerController.transform.position);

            if (distance < furthest)
            {
                furthest = distance;
                closestIndex = i;
            }
        }

        if (closestIndex == -1)
        {
            //Debug.LogError("Closest index is -1. FIX THIS BTICH!");
            targetType = SmogwalkerTarget.Player;
            return Vector3.zero;
        }

        Vector3 playerPos = GameProfileManager.Instance.gameProfiles[closestIndex].playerController.transform.position;

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
    [Server]
    private void Die()
    {
        WaveManager.Instance.RemoveEnemy();
        InstanceFinder.ServerManager.Despawn(gameObject);
    }
    [ObserversRpc]
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

            m_RendMat.SetFloat("_Flash", 1 - 4 * (timer - 0.5f) * (timer - 0.5f));

            timer += Time.deltaTime * 5;
            yield return null;
        }

        m_RendMat.SetFloat("_Flash", 0);

        visual.localScale = Vector3.one;
    }
    public void SetDamageTriggerActiveState(bool state)
    {
        m_DamageVolume.gameObject.SetActive(state);
    }
}