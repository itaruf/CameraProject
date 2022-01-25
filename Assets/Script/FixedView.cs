using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedView : Aview
{
    public float yaw;
    public float pitch;
    public float roll;
    public float fov;

    public override CameraConfiguration GetConfiguration() 
    {
        CameraConfiguration config = new CameraConfiguration();
        config.distance = 0;
        config.pivot = transform.position;
        return  config;
       
    }
}
