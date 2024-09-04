using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAdService : MonoBehaviour, IComparable<IAdService> {

    [SerializeField]
    private int priority;
    public int Priority { get { return priority; } set { priority = value; } }
    public abstract void Initialize();
    public abstract void Initialize(string[] args);
    public abstract bool ShowAds();
    public abstract void Cache();
    public abstract bool Personalized { get; set; }

    public event System.Action onAdClosed;

    protected void TriggerOnAdClosed() {
        if (onAdClosed != null)
            onAdClosed();
    }

    public int CompareTo(IAdService other)
    {
        return other.priority - priority;
    }
}
