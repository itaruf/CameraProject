using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DollyView : Aview
{
    public float roll, distance, fov;
    public float distanceOnRail, speed;

    public GameObject target;
    public Rail rail;

    public readonly float tolerance = 0.1f;

    public bool isAuto;

    public float[] distances;
    public int nearestNodeIndex = 0;
    public float min = float.MaxValue;
    public Vector3[] nearestPointsOnSegment;

    public List<KeyValuePair<Vector3, Vector3>> segments;

    public int currentStartingNodeIndex = 0;
    public int currentEndingNodeIndex = 0;
    public int initialStartingNodeIndex = 0;

    public int currentSegmentIndex = 0;
    public int segmentIndex = 0;

    private CameraConfiguration config;

    void Start()
    {
        config = GetConfiguration();
        config.pivot = transform.position = new Vector3(rail.nodesPos[0].x, rail.nodesPos[0].y, rail.nodesPos[0].z);

        distances = new float[rail.nodesPos.Count - 1];
        segments = new List<KeyValuePair<Vector3, Vector3>>();
        nearestPointsOnSegment = new Vector3[rail.nodesPos.Count - 1];

        for (int i = 0; i < rail.nodesPos.Count - 1; i++)
            segments.Add(new KeyValuePair<Vector3, Vector3>(rail.nodesPos[i], rail.nodesPos[i + 1]));

        int a = 0;
        for (int i = 0; i < rail.nodesPos.Count - 1; i++)
        {
            nearestPointsOnSegment[i] = MathUtils.GetNearestPointOnSegment(rail.nodesPos[i], rail.nodesPos[i + 1], target.transform.position);
            distances[i] = Vector3.Distance(nearestPointsOnSegment[i], target.transform.position);

            if (min > distances[i])
                a = i;
        }

        transform.position = nearestPointsOnSegment[a];
        currentStartingNodeIndex = a;
        currentEndingNodeIndex = currentStartingNodeIndex + 1;

        Array.Clear(nearestPointsOnSegment, 0, nearestPointsOnSegment.Length);
        Array.Clear(distances, 0, distances.Length);
    }

    public override CameraConfiguration GetConfiguration()
    {
        Vector3 dir = Vector3.Normalize(target.transform.position - transform.position);

        float yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;

        CameraConfiguration config = new CameraConfiguration
        {
            distance = 0,
            pivot = transform.position,
            yaw = yaw,
            pitch = pitch,
            roll = roll,
            fov = fov
        };

        return config;
    }
    void Update()
    {
        distanceOnRail = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float time = Time.deltaTime * speed;

        if (!isAuto)
        {
            if (Input.GetAxis("Horizontal") >= 0)
            {
                transform.position = Vector3.Lerp(transform.position, rail.nodesPos[currentEndingNodeIndex], distanceOnRail);
                transform.rotation = Quaternion.Lerp(transform.rotation, config.GetRotation(), distanceOnRail);

                config.roll = transform.rotation.eulerAngles.z;
                config.pitch = transform.rotation.eulerAngles.x;
                config.yaw = transform.rotation.eulerAngles.y;

                if (Vector3.Distance(transform.position, rail.nodesPos[currentEndingNodeIndex]) < tolerance)
                    if (currentEndingNodeIndex == rail.nodesPos.Count - 1)
                    {
                        currentStartingNodeIndex = initialStartingNodeIndex;
                        currentEndingNodeIndex = currentStartingNodeIndex + 1;

                        if (!rail.isLoop)
                            rail.nodesPos.Reverse();
                    }

                    else
                    {
                        currentStartingNodeIndex++;
                        currentEndingNodeIndex++;
                    }
            }
        }

        if (isAuto)
        {
            for (int i = 0; i < rail.nodesPos.Count - 1; i++)
            {
                nearestPointsOnSegment[i] = MathUtils.GetNearestPointOnSegment(rail.nodesPos[i], rail.nodesPos[i + 1], target.transform.position);
                distances[i] = Vector3.Distance(nearestPointsOnSegment[i], target.transform.position);

                if (min > distances[i])
                {
                    min = distances[i];
                    nearestNodeIndex = i;
                    segmentIndex = i;
                    currentSegmentIndex = i;
                }
            }

            if (time < 1)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, config.GetRotation(), time);

                config.roll = transform.rotation.eulerAngles.z;
                config.pitch = transform.rotation.eulerAngles.x;
                config.yaw = transform.rotation.eulerAngles.y;

                config.pivot = transform.position = Vector3.Lerp(transform.position, nearestPointsOnSegment[nearestNodeIndex], time);
            }

            else
            {
                transform.rotation = config.GetRotation();

                config.roll = transform.rotation.eulerAngles.z;
                config.pitch = transform.rotation.eulerAngles.x;
                config.yaw = transform.rotation.eulerAngles.y;

                config.pivot = transform.position = nearestPointsOnSegment[nearestNodeIndex];
            }

            // Clear
            min = float.MaxValue;
            Array.Clear(distances, 0, distances.Length);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        DrawGizmos(Color.black);
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 0.30f);

        if (nearestPointsOnSegment != null && nearestPointsOnSegment.Length > 0)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < nearestPointsOnSegment.Length; i++)
                Gizmos.DrawSphere(nearestPointsOnSegment[i], 0.25f);
        }
    }
}
