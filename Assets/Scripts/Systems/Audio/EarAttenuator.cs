using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarAttenuator : MonoBehaviour
{
    public static EarAttenuator Instance;
    public float leftRightAttenuation;
    public bool isOccluded = false;
    public int rayCount = 16;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        float finalAtten = 0;
        int collisionCount = 0;
        float checkAngle = 360f / rayCount;

        Vector3 finalDir = Vector3.zero;

        for (int i = 0; i < rayCount; i++)
        {
            Vector3 dir = Quaternion.AngleAxis(i * checkAngle, Vector3.up) * transform.forward;

            //TODO: use audio mask
            if (Physics.SphereCast(transform.position, 0.1f, dir, out RaycastHit hit, 8))
            {
                //finalAtten -= Vector3.Dot(dir.normalized, transform.right);
                //finalDir += dir.normalized;
                collisionCount++;
            }
            else
                finalDir += dir;
        }
        //finalAtten /= collisionCount;
        //finalAtten *= rayCount * 0.5f;
        //finalAtten = Mathf.Clamp(finalAtten, -1, 1);

        //finalDir /= collisionCount;
        //finalDir *= rayCount * 0.5f;

        if (collisionCount < rayCount * 0.5f)
        {
            finalAtten = 0;
        }
        else
        {
            finalDir = Vector3.ClampMagnitude(finalDir, 1);
            finalAtten = Vector3.Dot(finalDir, transform.right);
            finalAtten = Mathf.Clamp(finalAtten, -1, 1);
        }

        if (Physics.SphereCast(transform.position, 0.1f, Vector3.up, out RaycastHit hit2, 999))
        {
            isOccluded = collisionCount == rayCount;
        }
        else
        {
            finalAtten = 0;
            isOccluded = false;
        }

        leftRightAttenuation = Mathf.Lerp(leftRightAttenuation, finalAtten, Time.deltaTime * 5);
    }
    private void OnDrawGizmos()
    {
        float checkAngle = 360f / rayCount;
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 dir = Quaternion.AngleAxis(i * checkAngle, Vector3.up) * transform.forward;
            if (Physics.SphereCast(transform.position, 0.1f, dir, out RaycastHit hit, 8))
            {
                float dot = Vector3.Dot(dir.normalized, transform.right);
                dot *= 0.5f;
                dot += 0.5f;
                Gizmos.color = Color.Lerp(Color.blue, Color.red, dot);
                Gizmos.DrawSphere(hit.point, 0.1f);

                Gizmos.color = Color.green;
            }
            else
                Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, dir * 8);
        }
    }
}