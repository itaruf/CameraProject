using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AViewVolume : MonoBehaviour
{
    public int priority = 0;
    public Aview aview;
    protected bool IsActive { get; private set; }
    public virtual float ComputeSelfWeight()
    {
        return 1.0f;
    }

    protected void SetActive(bool isActive)
    {
        if (isActive)
            ViewVolumeBlender.instance.AddVolume(this);
        else
            ViewVolumeBlender.instance.RemoveVolume(this);
    }
}
