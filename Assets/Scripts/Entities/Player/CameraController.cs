using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public CameraEffects cameraEffects;
    public Transform camTarget;
    public Transform itemSpawnTransform;
    public Animator armsAnim;
    [SerializeField] private Transform m_CamHolder;
    [SerializeField] private float m_Sensitivity = 2.0f;

    private float m_X, m_Y;

    private void Awake()
    {
        Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (!camTarget)
            return;

        PlayerInput();
        HandleCamera();
    }
    private void PlayerInput()
    {
        m_X += Input.GetAxisRaw("Mouse X") * m_Sensitivity;
        m_Y -= Input.GetAxisRaw("Mouse Y") * m_Sensitivity;

        m_Y = Mathf.Clamp(m_Y, -90, 90);
    }
    private void HandleCamera()
    {
        m_CamHolder.position = camTarget.position;
        m_CamHolder.rotation = Quaternion.Euler(m_Y, m_X, 0);
    }
    public Vector3 GetCamForward()
    {
        return Quaternion.Euler(0, m_X, 0) * Vector3.forward;
    }
    public Vector3 GetCamRight()
    {
        return Quaternion.Euler(0, m_X, 0) * Vector3.right;
    }
}