using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmogwalkerChase : State<Smogwalker>
{
    public override void EnterState(Smogwalker ctx)
    {
        ctx.anims.SetBool("Attacking", false);
    }
    public override void ExitState(Smogwalker ctx)
    {
        
    }
    public override void FixedUpdateState(Smogwalker ctx)
    {
        
    }
    public override void UpdateState(Smogwalker ctx)
    {
        Vector3 targetPos = ctx.GetNearestTarget(out SmogwalkerTarget targetType);

        //Debug.Log($"Target Pos:{targetPos}, Target Type: {targetType}, Target Distance: {Vector3.Distance(ctx.transform.position, targetPos)}");

        ctx.agent.SetDestination(targetPos);

        float targetDistance = Vector3.Distance(ctx.transform.position, targetPos);

        targetDistance -= targetType == SmogwalkerTarget.Shield ? 1 : 0;

        targetDistance = Mathf.Max(targetDistance, 0);

        if (ctx.SwitchByCondition(SmogwalkerState.Attack, targetDistance < 1.5f))
            return;
    }
}