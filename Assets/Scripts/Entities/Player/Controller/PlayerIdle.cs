using UnityEngine;

public class PlayerIdle : State<PlayerController>
{
    private Vector3 m_InputVelocity;
    public override void EnterState(PlayerController ctx)
    {
        m_InputVelocity.x = ctx.lastVelocity.x;
        m_InputVelocity.z = ctx.lastVelocity.z;

        ctx.lastVelocity = Vector3.zero;

        ctx.gravityVector.y = -1;

        if (ctx.previousState != null && (PlayerState.Air & (PlayerState)ctx.previousState) != 0)
        {
            CameraController.Instance.cameraEffects.DoLanding();
        }
    }
    public override void ExitState(PlayerController ctx)
    {
        ctx.gravityVector.y = 0;
    }
    public override void FixedUpdateState(PlayerController ctx)
    {
        
    }
    public override void UpdateState(PlayerController ctx)
    {
        ctx.characterController.Move((m_InputVelocity + ctx.gravityVector) * Time.deltaTime);

        m_InputVelocity = Vector3.MoveTowards(m_InputVelocity, Vector3.zero, Time.deltaTime * 20);

        if (ctx.SwitchByCondition(PlayerState.Run, ctx.isUsingInput))
            return;
        if (ctx.SwitchByCondition(PlayerState.Jump, Input.GetKeyDown(KeyCode.Space)))
            return;
        if (ctx.SwitchByCondition(PlayerState.Airborne, !ctx.isGrounded))
            return;
    }
}