using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFollowView : Aview
{
    public float roll, fov, speed;
    public GameObject target;
    private Vector3 rot;

    // Constraints
    public GameObject centralPoint;

    [Range(-180, 180)] public float yawOffsetMax;
    [Range(-90, 90)] public float pitchOffsetMax;

    void Start()
    {


    }

    public override CameraConfiguration GetConfiguration()
    {

        Vector3 dir = Vector3.Normalize(target.transform.position - transform.position);

        /*rot.z = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        rot.y = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        transform.localScale = rot;*/

        // Constraints
        Vector3 dirToCentral = Vector3.Normalize(centralPoint.transform.position - transform.position);

        /*yawOffsetMax = Mathf.Atan2(dirToCentral.x, dirToCentral.z) * Mathf.Rad2Deg;
        pitchOffsetMax = -Mathf.Asin(dirToCentral.y) * Mathf.Rad2Deg;*/

        float yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float pitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;

        float yawCentral = Mathf.Atan2(dirToCentral.x, dirToCentral.z) * Mathf.Rad2Deg;
        float pitchCentral = -Mathf.Asin(dirToCentral.y) * Mathf.Rad2Deg;

        float yawDiff = yaw - yawCentral;
        float pitchDiff = pitch - pitchCentral;

        while (yawDiff > 180)
            yawDiff -= 360;

        while (yawDiff < -180)
            yawDiff += 360;

        yawDiff = Mathf.Clamp(yawDiff, -yawOffsetMax, yawOffsetMax);
        pitchDiff = Mathf.Clamp(pitchDiff, -pitchOffsetMax, pitchOffsetMax);

        CameraConfiguration config = new CameraConfiguration
        {
            distance = 0,
            pivot = transform.position,
            yaw = yawDiff + yawCentral,
            pitch = pitchDiff + pitchCentral,
            roll = roll,
            fov = fov,
        };

        return config;
    }
}
