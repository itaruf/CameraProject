using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public bool isLoop;
    private float length;
    private List<Vector3> positions;

    void Start()
    {
        positions = new List<Vector3>(transform.childCount);

        foreach (Transform child in transform)
            positions.Add(child.position);

        for (int i = 0; i < positions.Count - 1; i++)
            length += Vector3.Distance(positions[i], positions[i + 1]);

        if (isLoop)
            length += Vector3.Distance(positions[positions.Count - 1], positions[0]);

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
        positions[0]

    }*/
}
