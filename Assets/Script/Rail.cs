using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public bool isLoop;
    private float length = 0;
    public List<Vector3> nodesPos;
    public List<float> nodesDist;
    public List<float> nodesDistFromStart;

    void Start()
    {
        nodesPos = new List<Vector3>(transform.childCount);
        nodesDist = new List<float>();
        nodesDistFromStart = new List<float>();

        foreach (Transform child in transform)
        {
            nodesPos.Add(child.position);
        }

        for (int i = 0; i < nodesPos.Count - 1; i++)
        {
            nodesDist.Add(Vector3.Distance(nodesPos[i], nodesPos[i + 1]));
            length += nodesDist[i];

            nodesDistFromStart.Add(Vector3.Distance(nodesPos[0], nodesPos[i + 1]));
        }

        if (isLoop)
        {
            nodesPos.Add(nodesPos[0]);
            nodesDist.Add(Vector3.Distance(nodesPos[nodesPos.Count - 1], nodesPos[0]));
            length += nodesDist[nodesDist.Count - 1];

            nodesDistFromStart.Add(Vector3.Distance(nodesPos[nodesPos.Count - 1], nodesPos[0]));
        }
    }

    void Update()
    {
       
    }

    public float GetLength()
    {
        return length;
    }

    public Vector3 GetPosition(float distance)
    {
        Vector3 dir = Vector3.Normalize(nodesPos[0] + nodesPos[1]);
        Vector3 finalDir = dir * distance;

        return nodesPos[0] + finalDir;
    }

    public void OnDrawGizmos()
    {
        if (nodesPos.Count <= 0)
            return;

        for (int i = 0; i < nodesPos.Count - 1; i++)
        {
            DrawGizmos(Color.red, nodesPos[i], nodesPos[i + 1]);
        }

        if (isLoop)
            DrawGizmos(Color.red, nodesPos[nodesPos.Count - 1], nodesPos[0]);
    }
    public void DrawGizmos(Color color, Vector3 startingPoint, Vector3 endPoint)
    {
        Gizmos.color = color;

        Gizmos.DrawSphere(startingPoint, 0.25f);
        Gizmos.DrawSphere(endPoint, 0.25f);

        Gizmos.DrawLine(startingPoint, endPoint);
    }
}
