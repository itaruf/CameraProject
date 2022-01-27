using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static Vector3 GetNearestPointOnSegment(Vector3 A, Vector3 B, Vector3 target)
    {
        Vector3 AToTarget = target - A;
        Vector3 n = (B - A).normalized;

        Debug.Log(n);
        float scalar = Vector3.Dot(AToTarget, n);

        /*float scalar = AToTarget * n * Vector3.Dot(A + target, A + B) / (AToTarget + n);*/

        Debug.Log(scalar);

        scalar = Mathf.Clamp(scalar, 0, Vector3.Distance(A, B));

        Vector3 projC = A + n * scalar;

        Debug.Log(projC);
        return projC;
    }

    public static Vector3 LinearBezier(Vector3 A, Vector3 B, float t)
    {
        return (1 - t) * A + t * B;
    }

    public static Vector3 QuadraticBezier(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        return (1 - t) * (LinearBezier(A, B, t)) + t * LinearBezier(B, C, t);
    }

    public static Vector3 CubicBezier(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        return (1 - t) * (QuadraticBezier(A, B, C, t)) + t * QuadraticBezier(B, C, D, t);
    }
}
