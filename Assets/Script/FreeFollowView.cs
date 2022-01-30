using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowView : Aview
{
    public float[] pitch = new float[] { 0, 0, 0 };
    public float[] roll = new float[] { 0, 0, 0};
    public float[] fov = new float[] { 0, 0, 0 };

    [Range(0, 360)] public float yaw;
    public float yawSpeed;
    [Range(0,1)] public float curvePosition;
    public float curveSpeed;

    public GameObject target;
    public Curve curve;

    private CameraConfiguration freeFollowViewConfig;

    void Start()
    {
        freeFollowViewConfig = GetConfiguration();
    }

    void Update()
    {
        float time = Time.deltaTime * curveSpeed;

        yaw += Input.GetAxis("Horizontal") * yawSpeed;
        yaw = Mathf.Clamp(yaw, 0, 360);

        curvePosition += Input.GetAxis("Vertical") * curveSpeed;
        curvePosition = Mathf.Clamp(curvePosition, 0, 1);

        if (time < 1)
        {
            if (curvePosition >= 0 && curvePosition < 0.5f)
            {
                freeFollowViewConfig.roll = Mathf.Lerp(freeFollowViewConfig.roll, roll[0], time);
                freeFollowViewConfig.pitch = Mathf.Lerp(freeFollowViewConfig.pitch, pitch[0], time);
                freeFollowViewConfig.fov = Mathf.Lerp(freeFollowViewConfig.fov, fov[0], time);
            }

            if (curvePosition == 0.5f)
            {
                freeFollowViewConfig.roll = Mathf.Lerp(freeFollowViewConfig.roll, roll[1], time);
                freeFollowViewConfig.pitch = Mathf.Lerp(freeFollowViewConfig.pitch, pitch[1], time);
                freeFollowViewConfig.fov = Mathf.Lerp(freeFollowViewConfig.fov, fov[1], time);
            }

            if (curvePosition >= 0.5f && curvePosition <= 1f)
            {
                freeFollowViewConfig.roll = Mathf.Lerp(freeFollowViewConfig.roll, roll[2], time);
                freeFollowViewConfig.pitch = Mathf.Lerp(freeFollowViewConfig.pitch, pitch[2], time);
                freeFollowViewConfig.fov = Mathf.Lerp(freeFollowViewConfig.fov, fov[2], time);

            }

            freeFollowViewConfig.yaw = Mathf.Lerp(freeFollowViewConfig.yaw, yaw, time);
            freeFollowViewConfig.pivot = transform.position = Vector3.Lerp(freeFollowViewConfig.GetPosition(), curve.GetPosition(curvePosition), time);
        }

        else
        {
            if (curvePosition >= 0 && curvePosition < 0.5f)
            {
                freeFollowViewConfig.roll = roll[0];
                freeFollowViewConfig.pitch = pitch[0];
                freeFollowViewConfig.fov = fov[0];
            }

            if (curvePosition == 0.5f)
            {
                freeFollowViewConfig.roll = roll[1];
                freeFollowViewConfig.pitch = pitch[1];
                freeFollowViewConfig.fov = fov[1];
            }

            if (curvePosition >= 0.5f && curvePosition <= 1f)
            {
                freeFollowViewConfig.roll = roll[2];
                freeFollowViewConfig.pitch = pitch[2];
                freeFollowViewConfig.fov = fov[2];
            }

            freeFollowViewConfig.yaw = yaw;
            freeFollowViewConfig.pivot = transform.position = curve.GetPosition(curvePosition);


        }
        curve.gameObject.transform.position = target.transform.position;
    }

    public Matrix4x4 ComputeCurveToWorldMatrix()
    {
        Quaternion rotation = Quaternion.Euler(0, yaw, 0);
        return Matrix4x4.TRS(target.transform.position, rotation, Vector3.one);
    }

    public override CameraConfiguration GetConfiguration()
    {
        Vector3 dir = Vector3.Normalize(target.transform.position - transform.position);

        float pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;

        CameraConfiguration config = new CameraConfiguration
        {
            distance = 0,
            pivot = transform.position,
            yaw = yaw,
            pitch = this.pitch[1],
            roll = this.roll[1],
            fov = fov[1]
        };

        return config;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        DrawGizmos(Color.grey);
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        if (freeFollowViewConfig != null)
        {
            Gizmos.DrawSphere(freeFollowViewConfig.pivot, 0.25f);
            freeFollowViewConfig.DrawGizmos(color);
        }
    }
}
