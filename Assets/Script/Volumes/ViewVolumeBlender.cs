using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVolumeBlender : MonoBehaviour
{
    public List<AViewVolume> activeViewVolumes;
    public Dictionary<Aview, List<AViewVolume>> volumesPerViews;

    public static ViewVolumeBlender instance;
    private int max = int.MinValue;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    void Start()
    {
        activeViewVolumes = new List<AViewVolume>();
        volumesPerViews = new Dictionary<Aview, List<AViewVolume>>();
    }
    public void Update()
    {
        foreach (AViewVolume aViewVolume in activeViewVolumes)
        {
            aViewVolume.aview.weight = 0;

            if (max < aViewVolume.priority)
                max = aViewVolume.priority;
        }

        foreach (AViewVolume aViewVolume in activeViewVolumes)
        {
            if (aViewVolume.priority < max)
                aViewVolume.aview.weight = 0;

            else
                aViewVolume.aview.weight = Mathf.Max(aViewVolume.aview.weight, aViewVolume.ComputeSelfWeight());
        }
    }

    public void AddVolume(AViewVolume aViewVolume)
    {
        activeViewVolumes.Add(aViewVolume);

        if (!volumesPerViews.ContainsKey(aViewVolume.aview))
        {
            volumesPerViews.Add(aViewVolume.aview, new List<AViewVolume>());
            aViewVolume.aview.SetActive(true);
        }

        volumesPerViews[aViewVolume.aview].Add(aViewVolume);
    }

    public void RemoveVolume(AViewVolume aViewVolume)
    {
        activeViewVolumes.Remove(aViewVolume);

        volumesPerViews[aViewVolume.aview].Remove(aViewVolume);

        if (volumesPerViews[aViewVolume.aview].Count == 0)
        {
            volumesPerViews.Remove(aViewVolume.aview);
            aViewVolume.aview.SetActive(false);
        }
    }

    public List<AViewVolume> GetAViewVolumes()
    {
        return activeViewVolumes;
    }
}
