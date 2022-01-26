using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Camera camera;
    private Vector3 pos2;

    private List<Aview> activeViews = new List<Aview>();

    private FixedView currentConfig;
    public  FixedView targetConfig;
    public float speed;
    public float time;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FixedView fixedView = GetComponent<FixedView>();
        currentConfig = new FixedView();

        // Fixing position
        transform.position = new Vector3(fixedView.roll, fixedView.pitch, fixedView.yaw);
        camera.fieldOfView = fixedView.fov;

        // Current Fixed View
        currentConfig.weight = fixedView.weight;
        currentConfig.yaw = fixedView.yaw;
        currentConfig.pitch = fixedView.pitch;
        currentConfig.roll = fixedView.roll;
        currentConfig.fov = fixedView.fov;

        time = 0;

    }

    private void Update()
    {
        Debug.Log(transform.position);

        if (time < 1) {
            time = Time.deltaTime * 0.1f;
            transform.position = transform.position + (new Vector3(targetConfig.roll, targetConfig.pitch, targetConfig.yaw) - transform.position) * time;
        }
        else
            transform.position = new Vector3(targetConfig.roll, targetConfig.pitch, targetConfig.yaw);
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
        return sum/weights;
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
        return sum / weights;
    }

    public void addView(Aview view) 
    {
        activeViews.Add(view);
    
    }

    public void removeView(Aview aview) 
    {
        activeViews.Remove(aview);
    }

    


    public void ApplyConfiguration(Camera camera, CameraConfiguration configuration) 
    {
    
    
    }

}
