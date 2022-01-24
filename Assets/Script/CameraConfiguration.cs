using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
 public class CameraConfiguration
{
    public float yaw;
    public float pitch;
    public float roll;
    public Vector3 pivot;
    public float distance;
    public float fov;

    public Quaternion GetRotation() 
    {

        return Quaternion.Euler(pitch, yaw, roll);
    }

    public Vector3 GetPosition() 
    {
        Vector3 offset = GetRotation() *(Vector3.back * distance);

        return pivot + offset;
    }

}
