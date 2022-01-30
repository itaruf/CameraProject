using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour
{
    public GameObject A, B, C, D;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Vector3 GetPosition(float t)
    {
        return MathUtils.CubicBezier(A.transform.position, B.transform.position, C.transform.position, D.transform.position, t);
    }

    public Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix)
    {
        Vector3 pos = MathUtils.CubicBezier(A.transform.position, B.transform.position, C.transform.position, D.transform.position, t);
        return localToWorldMatrix.MultiplyPoint(pos);
    }

    public void OnDrawGizmos()
    {
        DrawGizmo(Color.blue, transform.localToWorldMatrix);
    }

    public void DrawGizmo(Color c, Matrix4x4 localToWorldMatrix)
    {
        Gizmos.color = c;

        Gizmos.DrawSphere(GetPosition(0), 0.25f);
        //Gizmos.DrawSphere(GetPosition(0.25f, localToWorldMatrix), 0.25f);
        Gizmos.DrawSphere(GetPosition(0.5f), 0.25f);
        //Gizmos.DrawSphere(GetPosition(0.75f, localToWorldMatrix), 0.25f);
        Gizmos.DrawSphere(GetPosition(1f), 0.25f);

    }
}
