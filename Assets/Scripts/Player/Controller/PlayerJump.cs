using UnityEngine;

public class PlayerJump : State<PlayerController>
{
    private float m_EnterY;
    private Vector3 m_MoveVelocity;
    private float m_EnterMagnitude;
    public override void EnterState(PlayerController ctx)
    {
        m_EnterY = ctx.transform.position.y;
        m_MoveVelocity.x = ctx.lastVelocity.x;
        m_MoveVelocity.z = ctx.lastVelocity.z;

        m_EnterMagnitude = Mathf.Max(m_MoveVelocity.magnitude, ctx.airSpeed);

        ctx.gravityVector.y = ctx.jumpForce;
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
        if (ctx.SwitchByCondition(PlayerState.Airborne, ctx.transform.position.y < m_EnterY))
            return;
    }
}