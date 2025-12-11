using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmogwalkerChase : State<Smogwalker>
{
    public override void EnterState(Smogwalker ctx)
    {
        
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
    }
}