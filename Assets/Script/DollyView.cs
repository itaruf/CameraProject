using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyView : Aview
{
    public float roll, distance, fov;
    public GameObject target;
    public Rail rail;
    public float distanceOnRail, speed;

    void Start()
    {
        
    }

    void Update()
    {

        /*distanceOnRail = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Debug.Log(distance);*/

        // 3ème noeud 
        // positions[2]

        /*for (int i = 0; i < )
            float time = Time.deltaTime * speed;

        transform.position = Vector3.Lerp(transform.position, ) * time;
        distanceOnRail = Vector3.Distance(transform.position, ) * time;

        if (time > 1)
        {
            time = 0;
            transform.position =
        }*/
    }

    public void OnDrawGizmos()
    {
        //DrawGizmos(Color.black);
    }
    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
