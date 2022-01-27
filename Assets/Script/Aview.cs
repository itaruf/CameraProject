using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Aview : MonoBehaviour
{
    public float weight;
    public bool isActiveOnStart;

    public virtual CameraConfiguration GetConfiguration() { return null; }

    private void Start()
    {
        if (isActiveOnStart)
        {
            SetActive(isActiveOnStart);
        }
        
    }

    public void SetActive(bool isActive) 
    {
        if (isActive) 
        {
            CameraController.instance.addView(this);
        }


        else
        {
            CameraController.instance.removeView(this);
        }
        
        
                           
    }
    public virtual void OnDrawGizmos()
    {
        CameraConfiguration cameraConfiguration = GetConfiguration();

        if (cameraConfiguration != null)
            cameraConfiguration.DrawGizmos(Color.green);
    }
}
