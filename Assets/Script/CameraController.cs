using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Camera camera;

    public CameraConfiguration initialCameraConfiguration;
    public List<Aview> activeViews = new List<Aview>();

    public float speed;

    // 2  - Fixed View et Moyenne
    public CameraConfiguration averageCameraConfiguration;

    // 3 - Smoothing
    public CameraConfiguration currentCameraConfiguration;
    public CameraConfiguration targetCameraConfiguration;

    // 4 – Fixed Position Follow
    public FixedFollowView fixedFollowView;

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
        currentCameraConfiguration = new CameraConfiguration
        {
            yaw = initialCameraConfiguration.yaw,
            pitch = initialCameraConfiguration.pitch,
            roll = initialCameraConfiguration.roll,
            fov = initialCameraConfiguration.fov,
            pivot = initialCameraConfiguration.pivot,
            distance = initialCameraConfiguration.distance
        };

        ApplyConfiguration(camera, currentCameraConfiguration);
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
        camera.transform.position = configuration.GetPosition();
        camera.transform.rotation = configuration.GetRotation();
        camera.fieldOfView = configuration.fov;
    }

    public void OnDrawGizmos()
    {
        currentCameraConfiguration.DrawGizmos(Color.red);
    }
}
