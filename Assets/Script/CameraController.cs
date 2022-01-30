using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Camera camera;

    public CameraConfiguration initialCameraConfig;
    public List<Aview> activeViews = new List<Aview>();

    public float speed;

    // 2  - Fixed View et Moyenne
    public CameraConfiguration averageCameraConfiguration;

    // 3 - Smoothing
    public CameraConfiguration currentCameraConfig;
    public CameraConfiguration targetCameraConfiguration;

    // 4 – Cut
    private bool isCutRequested = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Starting Config
        initialCameraConfig = new CameraConfiguration
        {
            yaw = transform.rotation.eulerAngles.y,
            pitch = transform.rotation.eulerAngles.x,
            roll = transform.rotation.eulerAngles.z,
            fov = camera.fieldOfView,
            pivot = transform.position,
            distance = 0,
        };

        camera.transform.position = initialCameraConfig.GetPosition();
        camera.transform.rotation = transform.rotation = initialCameraConfig.GetRotation();
        camera.fieldOfView = initialCameraConfig.fov;

        currentCameraConfig = initialCameraConfig;
    }

    private void Update()
    {

        // Test : 2 – Fixed View et Moyenne : Interpolation Config Moyenne
        averageCameraConfiguration = new CameraConfiguration
        {
            roll = ComputeAverageRoll(),
            pitch = ComputeAveragePitch(),
            yaw = ComputeAverageYaw(),
            pivot = ComputeAveragePivot(),
            distance = ComputeAverageDistance(),
            fov = ComputeAverageFov(),
        };

        Debug.Log(activeViews.Count);

        foreach (AViewVolume aviewVolume in ViewVolumeBlender.instance.activeViewVolumes)
            if (aviewVolume.isCutOnSwitch)
                Cut();

        ApplyConfiguration(camera, averageCameraConfiguration);
    }

    public void Cut()
    {
        //isCutRequested = false;

        Quaternion tmp = averageCameraConfiguration.GetRotation();

        currentCameraConfig.roll = tmp.eulerAngles.z;
        currentCameraConfig.pitch = tmp.eulerAngles.x;
        currentCameraConfig.yaw = tmp.eulerAngles.y;
        currentCameraConfig.pivot = averageCameraConfiguration.GetPosition();
        currentCameraConfig.fov = averageCameraConfiguration.fov;
        currentCameraConfig.distance = averageCameraConfiguration.distance;

        camera.transform.position = currentCameraConfig.GetPosition();
        camera.transform.rotation = currentCameraConfig.GetRotation();
        camera.fieldOfView = currentCameraConfig.fov;
    }

    public float ComputeAverageYaw()
    {
        Vector2 sum = Vector2.zero;

        foreach (Aview view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            sum += new Vector2(Mathf.Cos(config.yaw * Mathf.Deg2Rad),
                Mathf.Sin(config.yaw * Mathf.Deg2Rad)) * view.weight;
        }

        return Vector2.SignedAngle(Vector2.right, sum);
    }

    public float ComputeAverageRoll() 
    {
        float sum = 0;
        float weights = 0;

        foreach (Aview view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            weights += view.weight;
            sum += config.roll * view.weight;
               
        }

        if (weights == 0)
            return sum;

        return sum / weights;
    }

    public float ComputeAveragePitch()
    {
        float sum = 0;
        float weights = 0;

        foreach (Aview view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            weights += view.weight;
            sum += config.pitch * view.weight;

        }

        if (weights == 0)
            return sum;

        return sum / weights;
    }

    public float ComputeAverageDistance()
    {
        float sum = 0;
        float weights = 0;

        foreach (Aview view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            weights += view.weight;
            sum += config.distance * view.weight;
        }

        if (weights == 0)
            return sum;

        return sum / weights;
    }

    public float ComputeAverageFov()
    {
        float sum = 0;
        float weights = 0;

        foreach (Aview view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            weights += view.weight;
            sum += config.fov * view.weight;
        }

        if (weights == 0)
            return sum;

        return sum / weights;
    }

    public Vector3 ComputeAveragePivot()
    {
        List<Vector3> pos = new List<Vector3>();
        foreach (Aview view in activeViews) 
        {
            CameraConfiguration config = view.GetConfiguration();
            pos.Add(config.GetPosition());
        }

        if (pos.Count == 0)
            return Vector3.zero;

       return new Vector3(pos.Average(x => x.x), pos.Average(x => x.y), pos.Average(x => x.z));
    }

    public void AddView(Aview view) 
    {
        activeViews.Add(view);
    }

    public void RemoveView(Aview aview) 
    {
        activeViews.Remove(aview);
    }

    public void ApplyConfiguration(Camera camera, CameraConfiguration configuration) 
    {
        float time = Time.deltaTime * speed;

        if (time < 1)
        {
            Quaternion tmp = Quaternion.Lerp(currentCameraConfig.GetRotation(), configuration.GetRotation(), time);

            currentCameraConfig.roll = tmp.eulerAngles.z;
            currentCameraConfig.pitch = tmp.eulerAngles.x;
            currentCameraConfig.yaw = tmp.eulerAngles.y;
            currentCameraConfig.pivot = Vector3.Lerp(currentCameraConfig.GetPosition(), configuration.GetPosition(), time);
            currentCameraConfig.fov = Mathf.Lerp(currentCameraConfig.fov, configuration.fov, time);
            currentCameraConfig.distance = Mathf.Lerp(currentCameraConfig.distance, configuration.distance, time);

            camera.transform.position = currentCameraConfig.GetPosition();
            camera.transform.rotation = currentCameraConfig.GetRotation();
            camera.fieldOfView = currentCameraConfig.fov;

        }
        else
        {
            Quaternion tmp = configuration.GetRotation();

            currentCameraConfig.roll = tmp.eulerAngles.z;
            currentCameraConfig.pitch = tmp.eulerAngles.x;
            currentCameraConfig.yaw = tmp.eulerAngles.y;
            currentCameraConfig.pivot = configuration.GetPosition();
            currentCameraConfig.fov = configuration.fov;
            currentCameraConfig.distance = configuration.distance;

            camera.transform.position = currentCameraConfig.GetPosition();
            camera.transform.rotation = currentCameraConfig.GetRotation();
            camera.fieldOfView = currentCameraConfig.fov;
        }

        /*camera.transform.position = configuration.GetPosition();
        camera.transform.rotation = configuration.GetRotation();
        camera.fieldOfView = configuration.fov;*/
    }

    public void OnDrawGizmos()
    {
        currentCameraConfig.DrawGizmos(Color.red);
    }
}
