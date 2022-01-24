using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CameraController instance;

    public Camera camera;
    public CameraConfiguration configuration;

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
        DrawGizmos(Color.red);   
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(configuration.pivot, 0.25f);
        Vector3 position = configuration.GetPosition();
        Gizmos.DrawLine(configuration.pivot, position);
        Gizmos.matrix = Matrix4x4.TRS(position, configuration.GetRotation(), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, configuration.fov, 0.5f, 0f, Camera.main.aspect);
        Gizmos.matrix = Matrix4x4.identity;
    }


    public void ApplyConfiguration(Camera camera, CameraConfiguration configuration) 
    {
    
    
    }

}
