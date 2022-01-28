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
            SetActive(isActiveOnStart);
    }

    public void SetActive(bool isActive) 
    {
        if (isActive) 
            CameraController.instance.AddView(this);
        else
            CameraController.instance.RemoveView(this);
                           
    }
    public virtual void OnDrawGizmos()
    {
        CameraConfiguration cameraConfiguration = GetConfiguration();

        if (cameraConfiguration != null)
            cameraConfiguration.DrawGizmos(Color.green);
    }
}
