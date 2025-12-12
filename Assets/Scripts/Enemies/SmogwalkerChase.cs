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

        ctx.agent.SetDestination(targetPos);

        if (ctx.SwitchByCondition(SmogwalkerState.Attack, Vector3.Distance(ctx.transform.position, targetPos) < 1.5f))
            return;
    }
}