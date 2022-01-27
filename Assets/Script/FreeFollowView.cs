using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowView : Aview
{
    public float[] pitch = new float[] { 0, 0.5f, 1 };
    public float[] roll = new float[] { 0, 0.5f, 1 };
    public float[] fov = new float[] { 0, 0.5f, 1 };

    public float yaw, yawSpeed;
    [Range(0,1)] public float curvePosition;

    public GameObject target;
    public Curve curve;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public Matrix4x4 ComputeCurveToWorldMatrix()
    {
        Quaternion rotation = Quaternion.Euler(0, yaw, 0);
        return Matrix4x4.TRS(target.transform.position, rotation, Vector3.one);
    }
}
