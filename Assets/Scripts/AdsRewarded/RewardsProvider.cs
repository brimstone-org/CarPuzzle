using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RewardsProvider : MonoBehaviour, IComparable<RewardsProvider>
{

    [SerializeField]
    private int priority;
    public int Priority { get { return priority; } set { priority = value; } }

    [SerializeField]
    private List<PlacementMap> placements;
    protected List<PlacementMap> Placements { get { return placements; } }

    protected System.Action<string> AdStarted { get; set; }
    protected System.Action<string> AdCompleted { get; set; }
    protected System.Action<string> AdInterrupted { get; set; }
    protected System.Action<string> AdLoaded { get; set; }

    public abstract void RegisterCallbacks(System.Action<string> started, System.Action<string> completed, System.Action<string> interrupted, System.Action<string> adLoaded);

    public abstract void PlayAd();
    public abstract void PlayAd(string placement);
    public abstract void Cache();
    public abstract void Cache(string placement);
    public abstract void SetAutoCache(bool auto);
    public abstract bool IsAvailable();
    public abstract bool IsAvailable(string placement);
    public abstract bool Personalized { get; set; }

    public string FindPlacementId(string localPlacement)
    {
        for(int i=0; i < placements.Count; i++)
        {
            if (placements[i].localPlacement.Equals(localPlacement))
                return placements[i].placementId;
        }

        throw new System.Exception("Placement Id not found " + localPlacement);
    }

    public string FindPlacementLocal(string placementId)
    {
        for (int i = 0; i < placements.Count; i++)
        {
            if (placements[i].placementId.Equals(placementId))
                return placements[i].localPlacement;
        }

        throw new System.Exception("Placement local not found " + placementId);
    }

    public int CompareTo(RewardsProvider other)
    {
        return other.priority - priority;
    }
}

[System.Serializable]
public struct PlacementMap
{
    public string localPlacement;
    public string placementId;
}
