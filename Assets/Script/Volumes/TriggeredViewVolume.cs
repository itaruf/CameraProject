using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredViewVolume : AViewVolume
{
    private Collider collider;
    public GameObject target;
    public Color color;
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        if (collider == null)
            collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
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

    public void OnDrawGizmos()
    {
        DrawGizmos();
    }

    public void DrawGizmos()
    {
        Gizmos.color = this.color;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
