using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Aview : MonoBehaviour
{
    public float weight;

    public virtual CameraConfiguration GetConfiguration() { return null; }
}
