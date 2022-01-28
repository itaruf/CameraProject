using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public new Camera camera;
    private List<Aview> activeViews = new List<Aview>();
    private Vector3 targetConfig;
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

        CameraConfiguration config = new CameraConfiguration
        {
            yaw = fixedView.yaw,
            pitch = fixedView.pitch,
            roll = fixedView.roll,
            fov = fixedView.fov,
        };

        // Fixing position
        transform.position = new Vector3(config.roll, config.pitch, config.yaw);
        camera.fieldOfView = config.fov;

        time = 0;

        // Target
        targetConfig.z = ComputeAverageYaw();
        targetConfig.y = ComputeAveragePitch();
        targetConfig.x = ComputeAverageYaw();
    }

    private void Update()
    {
        /*if (time < 1) {
            time = Time.deltaTime * 0.1f;
            transform.position = transform.position + (new Vector3(targetConfig.roll, targetConfig.pitch, targetConfig.yaw) - transform.position) * time;
        }
        else
            transform.position = new Vector3(targetConfig.roll, targetConfig.pitch, targetConfig.yaw);*/

        Vector3 pos = transform.position;

        // 3 

        /*if (time < 1)
        {
            time = Time.deltaTime * 0.1f;
            //pos.z = transform.position.z + (targetConfig.yaw - transform.position.z) * time;
            pos.z = new Vector2(Mathf.Cos(transform.position.z + Mathf.Deg2Rad), Mathf.Sin(transform.position.z * Mathf.Deg2Rad)).y 
                + 
                (new Vector2(Mathf.Cos(targetConfig.yaw * Mathf.Deg2Rad), Mathf.Sin(targetConfig.yaw * Mathf.Deg2Rad)).y - new Vector2(Mathf.Cos(transform.position.z + Mathf.Deg2Rad), Mathf.Sin(transform.position.z * Mathf.Deg2Rad)).y) * time;
            pos.y = transform.position.y + (targetConfig.pitch - transform.position.y) * time;
            pos.x = transform.position.x + (targetConfig.roll - transform.position.x) * time;
        }
        else
            pos = new Vector3(targetConfig.roll, targetConfig.pitch, targetConfig.yaw);*/


        // 4

        /*float time = Time.deltaTime * speed;

        foreach (Aview activeView in activeViews)
        {
            FixedFollowView tmp = (FixedFollowView) activeView;

            if (tmp)
            {
                Debug.Log("in");

                if (time < 1)
                    tmp.pos = Vector3.Lerp(tmp.pos, tmp.target.transform.position, time);
                else
                    tmp.pos = tmp.target.transform.position;
            }
            break;
        }*/

        // 5


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
