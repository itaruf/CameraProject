using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFollowView : Aview
{
    public float roll, fov, speed;
    public GameObject target;
    public Vector3 pos;

    // Constraints
    public GameObject centralPoint;

    [Range(-180, 180)] public float yawOffsetMax;
    [Range(-90, 90)] public float pitchOffsetMax;

    void Start()
    {
        Vector3 dir = Vector3.Normalize(transform.position + target.transform.position);
        pos = transform.position;

        /*pos.z = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        pos.y = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;*/

        // Constraints

        Vector3 dirToCentral = Vector3.Normalize(transform.position + centralPoint.transform.position);

        /*yawOffsetMax = Mathf.Atan2(dirToCentral.x, dirToCentral.z) * Mathf.Rad2Deg;
        pitchOffsetMax = -Mathf.Asin(dirToCentral.y) * Mathf.Rad2Deg;*/

        float yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;

        float yawCentral = Mathf.Atan2(dirToCentral.x, dirToCentral.z) * Mathf.Rad2Deg;
        float pitchCentral = -Mathf.Asin(dirToCentral.y) * Mathf.Rad2Deg;

        float yawDiff = yaw - yawCentral;
        float pitchDiff = pitch - pitchCentral;

        pos.z = Mathf.Clamp(yawDiff, -180, 180);
        pos.y = Mathf.Clamp(pitchDiff, -90, 90);

    }

    void Update()
    {

    }
}
