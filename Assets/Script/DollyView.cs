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

        MathUtils.GetNearestPointOnSegment(rail.nodesPos[0], rail.nodesPos[1], target.transform.position);
    }

    void Update()
    {
        distanceOnRail = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        if (!isAuto)
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

    public void OnDrawGizmos()
    {
        DrawGizmos(Color.black);
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
