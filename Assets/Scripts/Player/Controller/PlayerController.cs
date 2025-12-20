using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState 
{ 
    Idle = 1, 
    Run = 2, 
    Jump = 4, 
    Airborne = 8, 
    Air = Jump | Airborne
}
public class PlayerController : StateMachine<PlayerController>
{
    public CharacterController characterController;
    public Transform visual;
    public float gravity = 20;
    public float moveSpeed = 4;
    public float airSpeed = 2.5f;
    public float maxAirSpeed = 4;
    public float jumpForce = 4;

    [HideInInspector] public float moveSpeedPerkValue;

    [Header("Movement Curves")]
    public AnimationCurve groundAccelerationCurve;
    public AnimationCurve landCurve;

    [Header("Camera")]
    public Transform cameraTarget;

    public Vector3 playerCenter => transform.position + Vector3.up * characterController.height * 0.5f;

    [HideInInspector] public Vector3 groundNormal;

    [HideInInspector] public Vector2 inputVector;
    [HideInInspector] public Vector3 moveVector;
    [HideInInspector] public Vector3 gravityVector;

    public bool isGrounded;
    [HideInInspector] public bool isUsingInput;
    [HideInInspector] public bool isMoving;

    [HideInInspector] public Vector3 lastVelocity;

    private float m_FOVMagTarget;

    private void Start()
    {
        PerksManager.OnPerksChanged += PerksManager_OnPerksChanged;
    }
    private void OnDestroy()
    {
        PerksManager.OnPerksChanged -= PerksManager_OnPerksChanged;
    }
    private void PerksManager_OnPerksChanged()
    {
        moveSpeedPerkValue = PerksManager.Instance.GetPerkValue(PerkType.Speed_Healing, 1);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
            return;

        CameraController.Instance.camTarget = cameraTarget;
        stateDictionary.Add(PlayerState.Idle, new PlayerIdle());
        stateDictionary.Add(PlayerState.Run, new PlayerRun());
        stateDictionary.Add(PlayerState.Jump, new PlayerJump());
        stateDictionary.Add(PlayerState.Airborne, new PlayerAirborne());

        SwitchState(PlayerState.Idle);

        moveSpeedPerkValue = 1;
    }
    public override void Update()
    {
        if (!IsOwner)
            return;

        if (transform.position.y <= -100)
        {
            transform.position = Vector3.up * 100;
        }
        if (GameProfile.Instance.playerHealth.dead.Value)
        {
            inputVector = Vector2.zero;
            return;
        }

        GroundCheck();

        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");

        moveVector = Vector3.Cross(CameraController.Instance.GetCamRight(), groundNormal) * inputVector.y - 
            Vector3.Cross(CameraController.Instance.GetCamForward(), groundNormal) * inputVector.x;

        visual.forward = CameraController.Instance.GetCamForward();

        moveVector = Vector3.ClampMagnitude(moveVector, 1);

        isUsingInput = inputVector.x != 0 || inputVector.y != 0;
        isMoving = Mathf.Abs(characterController.velocity.x) > 0.05f || Mathf.Abs(characterController.velocity.z) > 0.05f;

        m_FOVMagTarget = Mathf.Lerp(m_FOVMagTarget, Mathf.Max(isMoving ? inputVector.y : 0, 0), Time.deltaTime * 5);

        CameraController.Instance.cameraEffects.SetFOV(m_FOVMagTarget);
        CameraController.Instance.cameraEffects.SetLeanDirection(isMoving ? -inputVector.x : 0);

        base.Update();
    }
    private void GroundCheck()
    {
        RaycastHit hit;

        if (Physics.SphereCast(playerCenter, characterController.radius - 0.05f, Vector3.down, out hit, 
            characterController.height * 0.5f - characterController.radius + 0.1f + characterController.skinWidth, 
            GameManager.Instance.playerIgnoreMask))
        {
            isGrounded = true;
            groundNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            groundNormal = Vector3.up;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(playerCenter - Vector3.up * (characterController.height * 0.5f - characterController.radius + 0.1f + characterController.skinWidth), characterController.radius - 0.05f);
    }
}