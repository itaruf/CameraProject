using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AViewVolume : MonoBehaviour
{
    public int priority = 0;
    public Aview aview;
    protected bool IsActive { get; private set; }

    public bool isCutOnSwitch;
    public virtual float ComputeSelfWeight()
    {
        return 1.0f;
    }

    protected void SetActive(bool isActive)
    {
        if (isCutOnSwitch)
        {
            Debug.Log("Cut");
            ViewVolumeBlender.instance.Update();
            CameraController.instance.Cut();
        }

        if (isActive)
            ViewVolumeBlender.instance.AddVolume(this);
        else
            ViewVolumeBlender.instance.RemoveVolume(this);
    }
}
