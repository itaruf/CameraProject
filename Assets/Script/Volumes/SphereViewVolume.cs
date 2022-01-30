using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    public GameObject target;

    public float outerRadius;
    public float innerRadius;

    private float distance;
    void Start()
    {
        if (innerRadius > outerRadius)
            innerRadius = outerRadius;
    }

    void Update()
    {
        distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance <= outerRadius && !IsActive)
            SetActive(true);
        if (distance > outerRadius && IsActive)
            SetActive(false);
    }

    public override float ComputeSelfWeight()
    {
        return base.ComputeSelfWeight();
    }

    public void OnDrawGizmos()
    {
        DrawGizmos(Color.red, Color.green);
    }
    
    public void DrawGizmos(Color outerColor, Color innerColor)
    {
        if (innerRadius > outerRadius)
            innerRadius = outerRadius;

        Gizmos.color = outerColor;
        Gizmos.DrawWireSphere(transform.position, outerRadius);

        Gizmos.color = innerColor;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }
}
