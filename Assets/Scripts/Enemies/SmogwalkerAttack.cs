using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmogwalkerAttack : State<Smogwalker>
{
    public override void EnterState(Smogwalker ctx)
    {
        ctx.anims.SetBool("Attacking", true);
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

        if (ctx.SwitchByCondition(SmogwalkerState.Chase, Vector3.Distance(ctx.transform.position, targetPos) > 1.5f))
            return;
    }
}