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
    public float curveSpeed;

    public GameObject target;
    public Curve curve;

    void Start()
    {
        
    }

    void Update()
    {
        /*yaw = Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
        yaw = Mathf.Clamp(yaw, -90, 90);*/

        //yaw = Vector3.Lerp(transform.position.y, yaw, )

        yaw = Input.GetAxis("Horizontal");
        //transform.localRotation = new Quaternion(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, yaw, 1);
        curvePosition = Input.GetAxis("Vertical") * curveSpeed;
    }
    public Matrix4x4 ComputeCurveToWorldMatrix()
    {
        Quaternion rotation = Quaternion.Euler(0, yaw, 0);
        return Matrix4x4.TRS(target.transform.position, rotation, Vector3.one);
    }

    public override CameraConfiguration GetConfiguration()
    {
        Vector3 dir = Vector3.Normalize(target.transform.position - transform.position);

        float yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;

        CameraConfiguration config = new CameraConfiguration
        {
            distance = 0,
            pivot = transform.position,
            yaw = yaw,
            pitch = pitch,
            roll = this.roll[1],
            fov = fov[1]
        };

        return config;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        DrawGizmos(Color.blue);
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
