using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredViewVolume : AViewVolume
{
    private Collider collider;
    public GameObject target;
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        if (collider == null)
            collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("in");
        if (other.gameObject == target)
            SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("out");
        if (other.gameObject == target)
        {
            SetActive(false);
        }
    }

    private void OnGUI()
    {
        foreach(AViewVolume viewVolume in ViewVolumeBlender.instance.GetAViewVolumes())
        {
            GUILayout.Label(viewVolume.name);
        }
    }
}
