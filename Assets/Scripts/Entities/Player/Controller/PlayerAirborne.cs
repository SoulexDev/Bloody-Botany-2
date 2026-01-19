using UnityEngine;

public class PlayerAirborne : State<PlayerController>
{
    private Vector3 m_MoveVelocity;
    private float m_EnterMagnitude;
    public override void EnterState(PlayerController ctx)
    {
        m_MoveVelocity.x = ctx.lastVelocity.x;
        m_MoveVelocity.z = ctx.lastVelocity.z;

        m_EnterMagnitude = Mathf.Max(ctx.airSpeed, Mathf.Min(m_MoveVelocity.magnitude, ctx.maxAirSpeed * ctx.moveSpeed));
    }
    public override void ExitState(PlayerController ctx)
    {
        if (ctx.nextState != null && (PlayerState.Air & (PlayerState)ctx.nextState) == 0)
        {
            CameraController.Instance.cameraEffects.SetJumpDirection(0);
        }
    }
    public override void FixedUpdateState(PlayerController ctx)
    {

    }
    public override void UpdateState(PlayerController ctx)
    {
        m_MoveVelocity += ctx.moveVector * ctx.airSpeed * Time.deltaTime;
        m_MoveVelocity = Vector3.ClampMagnitude(m_MoveVelocity, m_EnterMagnitude);

        ctx.characterController.Move((m_MoveVelocity + ctx.gravityVector) * Time.deltaTime);
        ctx.gravityVector.y -= ctx.gravity * Time.deltaTime;

        ctx.lastVelocity = ctx.characterController.velocity;

        CameraController.Instance.cameraEffects.SetJumpDirection(Mathf.Clamp(ctx.characterController.velocity.y, -1, 2));

        if (ctx.SwitchByCondition(PlayerState.Run, ctx.isUsingInput && ctx.isGrounded && stateTime > 0.2f))
            return;
        if (ctx.SwitchByCondition(PlayerState.Idle, ctx.isGrounded && stateTime > 0.2f))
            return;
    }
}