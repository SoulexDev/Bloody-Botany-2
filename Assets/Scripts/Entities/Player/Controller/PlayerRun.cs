using UnityEngine;

public class PlayerRun : State<PlayerController>
{
    private float accelerationTimer = 0;
    private Vector2 lastInput;
    public override void EnterState(PlayerController ctx)
    {
        lastInput = Vector2.zero;
        accelerationTimer = ((PlayerState.Air & (PlayerState)ctx.previousState) == 0) ? 0 : 1;

        ctx.gravityVector.y = -1;
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
        if (Vector2.Dot(ctx.inputVector, lastInput) < 0)
            accelerationTimer = 0;

        ctx.characterController.Move((ctx.moveVector * ctx.moveSpeed * ctx.moveSpeedPerkValue *
            ctx.groundAccelerationCurve.Evaluate(accelerationTimer) + ctx.gravityVector) * Time.deltaTime);

        accelerationTimer += Time.deltaTime;
        accelerationTimer = Mathf.Clamp01(accelerationTimer);

        lastInput = ctx.inputVector;

        if (ctx.characterController.velocity != Vector3.zero)
            ctx.lastVelocity = ctx.characterController.velocity;

        if (ctx.SwitchByCondition(PlayerState.Idle, !ctx.isUsingInput))
            return;
        if (ctx.SwitchByCondition(PlayerState.Jump, Input.GetKeyDown(KeyCode.Space)))
            return;
        if (ctx.SwitchByCondition(PlayerState.Airborne, !ctx.isGrounded))
            return;
    }
}