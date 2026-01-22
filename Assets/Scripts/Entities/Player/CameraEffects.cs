using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    //[SerializeField] private Transform m_EffectTransform;
    [SerializeField] private CharacterController m_CharacterController;
    [SerializeField] private float m_FOVLower = 85, m_FOVUpper = 100;

    private Vector3 m_CamRotation;

    //private Vector3 m_EndPos;

    private void Awake()
    {
        //m_EndPos = m_OgPos + Vector3.down;
    }
    private void Update()
    {
        transform.localRotation = Quaternion.Euler(m_CamRotation);
    }
    public void SetLeanDirection(float direction)
    {
        m_CamRotation.z = Mathf.Lerp(m_CamRotation.z, direction * 2, Time.deltaTime * 5);
    }
    public void SetJumpDirection(float direction)
    {
        m_CamRotation.x = Mathf.Lerp(m_CamRotation.x, direction, Time.deltaTime * 10);
    }
    public void SetFOV(float magnitude)
    {
        Camera.main.fieldOfView = Mathf.Lerp(m_FOVLower, m_FOVUpper, magnitude);
    }
    public void DoLanding()
    {
        //StartCoroutine(DoLandCurve());
    }
    private IEnumerator DoLandCurve()
    {
        float timer = 0;

        Vector3 endPos = transform.InverseTransformPoint(transform.position - Vector3.up);
        while (timer < 1)
        {
            float eval = GameProfile.Instance.playerController.landCurve.Evaluate(timer);

            transform.localPosition = Vector3.Lerp(Vector3.zero, endPos, -eval);
            SetJumpDirection(-eval * 5);

            timer += Time.deltaTime * 2;
            yield return null;
        }

        transform.localPosition = Vector3.zero;
        SetJumpDirection(0);
    }
}