using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Aview : MonoBehaviour
{
    public float weight;
    public bool isActiveOnStart;

    public virtual CameraConfiguration GetConfiguration() { return null; }

    private void Start()
    {
        if (isActiveOnStart)
        {
            SetActive(isActiveOnStart);
        }
    }

    public void SetActive(bool isActive) 
    {
        gameObject.SetActive(isActive);
                           
    }
}
