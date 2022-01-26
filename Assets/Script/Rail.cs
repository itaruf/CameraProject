using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public bool isLoop;
    private float length = 0;
    public List<Vector3> positions;
    public List<float> distances;

    void Start()
    {
        positions = new List<Vector3>(transform.childCount);
        distances = new List<float>();

        foreach (Transform child in transform)
        {
            positions.Add(child.position);
        }

        for (int i = 0; i < positions.Count - 1; i++)
        {
            distances.Add(Vector3.Distance(positions[i], positions[i + 1]));
            length += distances[i];
        }

        if (isLoop)
        {
            distances.Add(Vector3.Distance(positions[positions.Count - 1], positions[0]));
            length += distances[distances.Count - 1];
        }

        Debug.Log(GetLength());
    }

    void Update()
    {
       
    }

    public float GetLength()
    {
        return length;
    }

    /*public Vector3 GetPosition(float distance)
    {

    }*/

    public void OnDrawGizmos()
    {
        for (int i = 0; i < positions.Count - 1; i++)
        {
            DrawGizmos(Color.red, positions[i], positions[i + 1]);
        }
        if (isLoop)
            DrawGizmos(Color.red, positions[positions.Count - 1], positions[0]);
    }
    public void DrawGizmos(Color color, Vector3 startingPoint, Vector3 endPoint)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(startingPoint, 0.25f);
        Gizmos.DrawLine(startingPoint, endPoint);
    }
}
