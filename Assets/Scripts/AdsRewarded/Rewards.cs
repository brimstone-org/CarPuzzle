using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDPR;

public class Rewards : MonoBehaviour {

    public static Rewards Instance { get; private set; }

    [SerializeField]
    private bool dontDestroyOnLoad = true;
    [SerializeField]
    private List<RewardsProvider> providers;
    [SerializeField]
    private bool autoCache = true;

    public static event System.Action<string> OnAdStarted, OnAdCompleted, OnAdInterrupted, OnAdLoaded;

    private int currentProvider;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

        } else
        {
            Destroy(gameObject);
        }
    } 

    private IEnumerator Start()
    {
        foreach(var p in providers)
        {
            p.Personalized = GDPRConsentPanel.Instance.AsBool;
            p.RegisterCallbacks(TriggerOnAdStarted, TriggerOnAdCompleted, TriggerOnAdInterrupted, TriggerOnAdLoaded);
            p.SetAutoCache(autoCache);
        }

        yield return new WaitForSeconds(1);
        PrioritySort();
    }

    public void UpdatePersonalized()
    {
        foreach (var p in providers)
        {
            p.Personalized = GDPRConsentPanel.Instance.AsBool;
        }
    }

    private void OnEnable()
    {
        GDPRConsentPanel.Instance.OnConsentChange += UpdatePersonalized;
    }

    private void OnDisable()
    {
        GDPRConsentPanel.Instance.OnConsentChange -= UpdatePersonalized;
    }

    public void PrioritySort()
    {
        providers.Sort();
    }

    public void PlayAd()
    {
        PrioritySort();
        foreach (var p in providers)
        {
            if (p.IsAvailable())
            {
                p.PlayAd();
                return;
            }
        }
    }

    public void PlayAd(string placement)
    {
        PrioritySort();
        foreach (var p in providers)
        {
            if (p.IsAvailable(placement))
            {
                p.PlayAd(placement);
                return;
            }
        }
    }

    private int NextProvider()
    {
        if (currentProvider + 1 == providers.Count)
            return 0;
        return currentProvider + 1;
    }

    public void TriggerOnAdStarted(string placement)
    {
        if (OnAdStarted != null)
            OnAdStarted(placement);
    }

    public void TriggerOnAdCompleted(string placement)
    {
        if (OnAdCompleted != null)
            OnAdCompleted(placement);
    }

    public void TriggerOnAdInterrupted(string placement)
    {
        if (OnAdInterrupted != null)
            OnAdInterrupted(placement);
    }

    public void TriggerOnAdLoaded(string placement)
    {
        if (OnAdLoaded != null)
            OnAdLoaded(placement);
    }

    public bool IsAvailable()
    {
        foreach(var p in providers)
        {
            if (p.IsAvailable())
                return true;
        }

        return false;
    }

    public bool IsAvailable(string placement)
    {
        foreach (var p in providers)
        {
            if (p.IsAvailable(placement))
                return true;
        }

        return false;
    }
}
