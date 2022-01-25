using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CameraController instance;

    public Camera camera;
    

    private List<Aview> activeViews = new List<Aview>();
    private List<CameraConfiguration> configurations = new List<CameraConfiguration>();

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
          
    }

    private void Update()
    {
        
    }

    public float ComputeAverageYaw()
    {
        Vector2 sum = Vector2.zero;
        foreach (CameraConfiguration config in configurations)
        {
            sum += new Vector2(Mathf.Cos(config.yaw * Mathf.Deg2Rad),
            Mathf.Sin(config.yaw * Mathf.Deg2Rad)) * config.weight;
        }
        return Vector2.SignedAngle(Vector2.right, sum);
    }

    void addView(Aview view) 
    {
        activeViews.Add(view);
    
    }

    void removeView(Aview aview) 
    {
        activeViews.Remove(aview);
    }

    


    public void ApplyConfiguration(Camera camera, CameraConfiguration configuration) 
    {
    
    
    }

}
