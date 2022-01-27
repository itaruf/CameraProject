using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyView : Aview
{
    public float roll, distance, fov;
    public float distanceOnRail, speed;

    public GameObject target;
    public Rail rail;

    private Vector3 pos;

    private Vector3 rot;

    private int currentStartingNodeIndex = 0;
    private int currentEndingNodeIndex = 0;
    private float tolerance = 0.01f;

    public int initialStartingNodeIndex = 0;

    public bool isAuto;
    private float[] distances;
    private int nearestNodeIndex = 0;
    private float min = float.MaxValue;
    private int[] indexes;
    
    void Start()
    {
        rot = transform.localScale;
        pos = transform.position = rail.nodesPos[0];

        Vector3 dir = Vector3.Normalize(transform.position - target.transform.position);

        rot.z = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        rot.y = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;

        transform.localScale = rot;

        currentStartingNodeIndex = initialStartingNodeIndex;
        currentEndingNodeIndex = currentStartingNodeIndex + 1;

        //MathUtils.GetNearestPointOnSegment(rail.nodesPos[0], rail.nodesPos[1], target.transform.position);

        distances = new float[rail.nodesPos.Count - 1];
    }

    void Update()
    {
        distanceOnRail = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        float time = Time.deltaTime * speed;
        Vector3[] nearestPointOnSegment = new Vector3[rail.nodesPos.Count - 1];

        Debug.Log(MathUtils.GetNearestPointOnSegment(rail.nodesPos[0], rail.nodesPos[1], target.transform.position));

        if (isAuto)
        {
            /*for (int i = 0; i < rail.nodesPos.Count - 1; i++)
            {
                nearestPointOnSegment[i] = MathUtils.GetNearestPointOnSegment(rail.nodesPos[i], rail.nodesPos[i + 1], target.transform.position);
                distances[i] = Vector3.Distance(rail.nodesPos[i], nearestPointOnSegment[i]);

                if (min > distances[i])
                {
                    min = distances[i];
                    nearestNodeIndex = i;
                }
            }

            if (time < 1)
            {
                pos = Vector3.MoveTowards(transform.position, rail.nodesPos[0] * distances[0], time);
                transform.position = pos;
            }

            else
            {
                transform.position = rail.nodesPos[0] * distances[0];
            }*/
        }

        else
        {
            if (Input.GetAxis("Horizontal") >= 0)
            {
                pos = Vector3.MoveTowards(transform.position, rail.nodesPos[currentEndingNodeIndex], distanceOnRail);

                transform.position = pos;

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
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        DrawGizmos(Color.black);
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 0.25f);

        if (rail.nodesPos.Count > 0 && target != null)
            Gizmos.DrawSphere(MathUtils.GetNearestPointOnSegment(rail.nodesPos[0], rail.nodesPos[1], target.transform.position), 0.25f);
    }
}
