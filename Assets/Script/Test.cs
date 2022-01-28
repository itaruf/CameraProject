using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Curve curve;

    void Start()
    {
        // Test : 2 – Fixed View et Moyenne
        /*CameraConfiguration averageCameraConfiguration = new CameraConfiguration
        {
            roll = CameraController.instance.ComputeAverageRoll(),
            pitch = CameraController.instance.ComputeAveragePitch(),
            yaw = CameraController.instance.ComputeAverageYaw(),
            pivot = CameraController.instance.ComputeAveragePivot(),
            distance = CameraController.instance.ComputeAverageDistance(),
            fov = CameraController.instance.ComputeAverageFov(),
        };

        CameraController.instance.ApplyConfiguration(camera averageCameraConfiguration);*/
    }

    void Update()
    {
        //TestFixedView();
        //TestSmoothing();
        TestFixedCamera();
    }

    private void OnDrawGizmos()
    {
        //curve.DrawGizmo(Color.green, transform.localToWorldMatrix);
    }

    private void TestFixedView()
    {
        float time = Time.deltaTime * CameraController.instance.speed;

        // Test : 2 – Fixed View et Moyenne : Interpolation Config Moyenne
        CameraConfiguration averageCameraConfiguration = CameraController.instance.averageCameraConfiguration;
        CameraConfiguration currentCameraConfig = CameraController.instance.currentCameraConfiguration;

        if (time < 1)
        {
            Quaternion tmp = Quaternion.Lerp(currentCameraConfig.GetRotation(), averageCameraConfiguration.GetRotation(), time);

            currentCameraConfig.roll = tmp.eulerAngles.z;
            currentCameraConfig.pitch = tmp.eulerAngles.x;
            currentCameraConfig.yaw = tmp.eulerAngles.y;
            currentCameraConfig.pivot = Vector3.Lerp(currentCameraConfig.GetPosition(), averageCameraConfiguration.GetPosition(), time);
            currentCameraConfig.fov = Mathf.Lerp(currentCameraConfig.fov, averageCameraConfiguration.fov, time);
            currentCameraConfig.distance = Mathf.Lerp(currentCameraConfig.distance, averageCameraConfiguration.distance, time);
        }
        else
        {
            Quaternion tmp = averageCameraConfiguration.GetRotation();

            currentCameraConfig.roll = tmp.eulerAngles.z;
            currentCameraConfig.pitch = tmp.eulerAngles.x;
            currentCameraConfig.yaw = tmp.eulerAngles.y;
            currentCameraConfig.pivot = averageCameraConfiguration.GetPosition();
            currentCameraConfig.fov = averageCameraConfiguration.fov;
            currentCameraConfig.distance = averageCameraConfiguration.distance;
        }

        CameraController.instance.ApplyConfiguration(CameraController.instance.camera, currentCameraConfig);
    }

    private void TestSmoothing()
    {
        float time = Time.deltaTime * CameraController.instance.speed;

        // Test : 3 – Smoothing
        CameraConfiguration currentCameraConfig = CameraController.instance.currentCameraConfiguration;
        CameraConfiguration targetCameraConfig = CameraController.instance.averageCameraConfiguration;

        if (time < 1)
        {
            currentCameraConfig.roll = currentCameraConfig.roll + (targetCameraConfig.roll - currentCameraConfig.roll) * time;
            currentCameraConfig.pitch = currentCameraConfig.pitch + (targetCameraConfig.pitch - currentCameraConfig.pitch) * time;
            currentCameraConfig.yaw = currentCameraConfig.yaw + (targetCameraConfig.yaw - currentCameraConfig.yaw) * time;
            currentCameraConfig.pivot = currentCameraConfig.pivot + (targetCameraConfig.GetPosition() - currentCameraConfig.pivot) * time;
            currentCameraConfig.fov = currentCameraConfig.fov + (targetCameraConfig.fov - currentCameraConfig.fov) * time;
            currentCameraConfig.distance = currentCameraConfig.distance + (targetCameraConfig.distance - currentCameraConfig.distance) * time;
        }
        else
        {
            currentCameraConfig.roll = targetCameraConfig.roll;
            currentCameraConfig.pitch = targetCameraConfig.pitch;
            currentCameraConfig.yaw = targetCameraConfig.yaw;
            currentCameraConfig.pivot = targetCameraConfig.GetPosition();
            currentCameraConfig.fov = targetCameraConfig.fov;
            currentCameraConfig.distance = targetCameraConfig.distance;
        }

        CameraController.instance.ApplyConfiguration(CameraController.instance.camera, currentCameraConfig);
    }

    private void TestFixedCamera()
    {

        FixedFollowView fixedFollow = CameraController.instance.fixedFollowView;

        float time = Time.deltaTime * fixedFollow.speed;

        // Test : 4 – Fixed View et Moyenne : Interpolation Config Moyenne
        CameraConfiguration fixedFollowConfig = fixedFollow.GetConfiguration();
        CameraConfiguration currentCameraConfig = CameraController.instance.currentCameraConfiguration;

        if (time < 1)
        {
            Quaternion tmp = Quaternion.Lerp(currentCameraConfig.GetRotation(), fixedFollowConfig.GetRotation(), time);

            currentCameraConfig.roll = tmp.eulerAngles.z;
            currentCameraConfig.pitch = tmp.eulerAngles.x;
            currentCameraConfig.yaw = tmp.eulerAngles.y;
            currentCameraConfig.pivot = Vector3.Lerp(currentCameraConfig.GetPosition(), fixedFollowConfig.GetPosition(), time);
            currentCameraConfig.fov = Mathf.Lerp(currentCameraConfig.fov, fixedFollowConfig.fov, time);
            currentCameraConfig.distance = Mathf.Lerp(currentCameraConfig.distance, fixedFollowConfig.distance, time);
        }
        else
        {
            Quaternion tmp = fixedFollowConfig.GetRotation();

            currentCameraConfig.roll = tmp.eulerAngles.z;
            currentCameraConfig.pitch = tmp.eulerAngles.x;
            currentCameraConfig.yaw = tmp.eulerAngles.y;
            currentCameraConfig.pivot = fixedFollowConfig.GetPosition();
            currentCameraConfig.fov = fixedFollowConfig.fov;
            currentCameraConfig.distance = fixedFollowConfig.distance;
        }

        CameraController.instance.ApplyConfiguration(CameraController.instance.camera, currentCameraConfig);
    }
}
