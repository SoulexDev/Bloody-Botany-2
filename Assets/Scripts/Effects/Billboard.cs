using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [Flags] private enum BillboardAxis { X = 1, Y = 2, Z = 4 }
    private enum BillboardMode { Directional, Facing }
    [SerializeField] private BillboardAxis m_BillboardAxis;
    [SerializeField] private BillboardMode m_BillboardMode;

    private void LateUpdate()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 camDir = Camera.main.transform.forward;
        Vector3 pos = transform.position;
        Vector3 axisScalar = new Vector3(
            (m_BillboardAxis & BillboardAxis.X) == 0 ? 0 : 1,
            (m_BillboardAxis & BillboardAxis.Y) == 0 ? 0 : 1,
            (m_BillboardAxis & BillboardAxis.Z) == 0 ? 0 : 1);

        camPos.Scale(axisScalar);
        camDir.Scale(axisScalar);
        pos.Scale(axisScalar);

        Vector3 direction = m_BillboardMode switch
        {
            BillboardMode.Directional => camPos - pos,
            BillboardMode.Facing => -camDir,
            _ => Vector3.zero
        };

        transform.rotation = Quaternion.LookRotation(-direction);
    }
}