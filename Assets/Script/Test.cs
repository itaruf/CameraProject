using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Curve curve;

    // 4 – Fixed Position Follow
    public FixedFollowView fixedFollowView;

    // 5/6 - Dolly camera automatique
    public DollyView dollyView;

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
        //TestFixedCamera();
        //TestDollyCamera();
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
        CameraConfiguration currentCameraConfig = CameraController.instance.currentCameraConfig;

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
        CameraConfiguration currentCameraConfig = CameraController.instance.currentCameraConfig;
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
        float time = Time.deltaTime * fixedFollowView.speed;

        // Test : 4 – Fixed View et Moyenne : Interpolation Config Moyenne
        CameraConfiguration fixedFollowConfig = fixedFollowView.GetConfiguration();
        CameraConfiguration currentCameraConfig = CameraController.instance.currentCameraConfig;

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

    private void TestDollyCamera()
    {
        dollyView.distanceOnRail = Input.GetAxis("Horizontal") * dollyView.speed * Time.deltaTime;
        float time = Time.deltaTime * dollyView.speed;

        CameraConfiguration dollyViewConfig = dollyView.GetConfiguration();
        CameraConfiguration currentCameraConfig = CameraController.instance.currentCameraConfig;


        if (!dollyView.isAuto)
        {
            if (Input.GetAxis("Horizontal") >= 0)
            {
                if (time < 1)
                {
                    Quaternion tmp = Quaternion.Lerp(currentCameraConfig.GetRotation(), dollyViewConfig.GetRotation(), time);

                    currentCameraConfig.roll = tmp.eulerAngles.z;
                    currentCameraConfig.pitch = tmp.eulerAngles.x;
                    currentCameraConfig.yaw = tmp.eulerAngles.y;
                    currentCameraConfig.pivot = Vector3.Lerp(currentCameraConfig.pivot, dollyView.rail.nodesPos[dollyView.currentEndingNodeIndex], dollyView.distanceOnRail);
                    currentCameraConfig.fov = Mathf.Lerp(currentCameraConfig.fov, dollyViewConfig.fov, time);
                    currentCameraConfig.distance = Mathf.Lerp(currentCameraConfig.distance, dollyViewConfig.distance, time);
                }
                else
                {
                    Quaternion tmp = dollyViewConfig.GetRotation();

                    currentCameraConfig.roll = tmp.eulerAngles.z;
                    currentCameraConfig.pitch = tmp.eulerAngles.x;
                    currentCameraConfig.yaw = tmp.eulerAngles.y;
                    currentCameraConfig.pivot = dollyView.rail.nodesPos[dollyView.currentEndingNodeIndex];
                    currentCameraConfig.fov = dollyViewConfig.fov;
                    currentCameraConfig.distance = dollyViewConfig.distance;
                }

                if (Vector3.Distance(currentCameraConfig.pivot, dollyView.rail.nodesPos[dollyView.currentEndingNodeIndex]) < dollyView.tolerance)
                    if (dollyView.currentEndingNodeIndex == dollyView.rail.nodesPos.Count - 1)
                    {
                        dollyView.currentStartingNodeIndex = dollyView.initialStartingNodeIndex;
                        dollyView.currentEndingNodeIndex = dollyView.currentStartingNodeIndex + 1;

                        if (!dollyView.rail.isLoop)
                            dollyView.rail.nodesPos.Reverse();
                    }

                    else
                    {
                        dollyView.currentStartingNodeIndex++;
                        dollyView.currentEndingNodeIndex++;
                    }
            }
            CameraController.instance.ApplyConfiguration(CameraController.instance.camera, currentCameraConfig);
        }

        else
        {
            for (int i = 0; i < dollyView.rail.nodesPos.Count - 1; i++)
            {
                dollyView.nearestPointsOnSegment[i] = MathUtils.GetNearestPointOnSegment(dollyView.rail.nodesPos[i], dollyView.rail.nodesPos[i + 1], dollyView.target.transform.position);
                dollyView.distances[i] = Vector3.Distance(dollyView.nearestPointsOnSegment[i], dollyView.target.transform.position);

                if (dollyView.min > dollyView.distances[i])
                {
                    dollyView.min = dollyView.distances[i];
                    dollyView.nearestNodeIndex = i;
                    dollyView.segmentIndex = i;
                    dollyView.currentSegmentIndex = i;
                }
            }

            if (time < 1)
            {
                Quaternion tmp = Quaternion.Lerp(currentCameraConfig.GetRotation(), dollyViewConfig.GetRotation(), time);

                currentCameraConfig.roll = tmp.eulerAngles.z;
                currentCameraConfig.pitch = tmp.eulerAngles.x;
                currentCameraConfig.yaw = tmp.eulerAngles.y;
                currentCameraConfig.pivot = Vector3.Lerp(currentCameraConfig.pivot, dollyView.nearestPointsOnSegment[dollyView.nearestNodeIndex], time);
                currentCameraConfig.fov = Mathf.Lerp(currentCameraConfig.fov, dollyViewConfig.fov, time);
                currentCameraConfig.distance = Mathf.Lerp(currentCameraConfig.distance, dollyViewConfig.distance, time);
            }

            else
            {
                Quaternion tmp = dollyViewConfig.GetRotation();

                currentCameraConfig.roll = tmp.eulerAngles.z;
                currentCameraConfig.pitch = tmp.eulerAngles.x;
                currentCameraConfig.yaw = tmp.eulerAngles.y;
                currentCameraConfig.pivot = dollyView.nearestPointsOnSegment[dollyView.nearestNodeIndex];
                currentCameraConfig.fov = dollyViewConfig.fov;
                currentCameraConfig.distance = dollyViewConfig.distance;
            }

            CameraController.instance.ApplyConfiguration(CameraController.instance.camera, currentCameraConfig);

            // Clear
            dollyView.min = float.MaxValue;
            Array.Clear(dollyView.distances, 0, dollyView.distances.Length);
        }
    }
}
