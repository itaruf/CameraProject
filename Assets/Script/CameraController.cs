using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Camera camera;
    private Vector3 pos2;
    public FixedView currentConfig;
    public FixedView targetConfig;
   
    private List<Aview> activeViews = new List<Aview>();
    

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
        //pos2 = new Vector3(ComputeAverageRoll(), ComputeAveragePitch(), ComputeAverageYaw());
        
         
    }

   

    private void Update()
    {
        
        /*float T = Time.deltaTime ;
         transform.position = Vector3.Lerp(transform.position, pos2, T);*/
        
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
