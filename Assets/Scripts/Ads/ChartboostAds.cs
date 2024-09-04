using UnityEngine;
using System.Collections;
using ChartboostSDK;

public class ChartboostAds : IAdService
{
    private bool personalized;
    public override bool Personalized
    {
        get { return personalized; }
        set { personalized = value;}
    }

    // Use this for initialization
    public override void Initialize()
    {
        // DDebug.LogError("CHARTBOOST:" + Chartboost.isInitialized());
        Chartboost.restrictDataCollection(personalized);
        Chartboost.didFailToLoadInterstitial += DidFailToLoadInterstitial;
        Chartboost.didCloseInterstitial += Chartboost_didCloseInterstitial;
        Chartboost.didCacheInterstitial += Chartboost_didCacheInterstitial;
        Chartboost.setAutoCacheAds(false);
        Chartboost.cacheInterstitial(CBLocation.Default);
    }

    void OnDisable(){
        Chartboost.didFailToLoadInterstitial -= DidFailToLoadInterstitial;
        Chartboost.didCloseInterstitial -= Chartboost_didCloseInterstitial;
        Chartboost.didCacheInterstitial -= Chartboost_didCacheInterstitial;
    }

    private void Chartboost_didCloseInterstitial(CBLocation obj)
    {
        TriggerOnAdClosed();
    }

    public override void Initialize(string[] args)
    {
        Initialize();
    }

    public override bool ShowAds()
    {
        Debug.Log("Chartboost: Trying to show ads");
        if (Chartboost.hasInterstitial(CBLocation.Default))
        {
            Chartboost.showInterstitial(CBLocation.Default);

            Debug.Log("Chartboost: Ad Shown at " + CBLocation.Default);
            return true;
        }
        Debug.Log("Chartboost: Failed to show Add");
        return false;

    }

    public override void Cache()
    {

        if (Chartboost.hasInterstitial(CBLocation.Default))
            return;

        Debug.Log("Chartboost: Caching");
        Chartboost.cacheInterstitial(CBLocation.Default);
    }

    void DidFailToLoadInterstitial(CBLocation location, CBImpressionError error)
    {
        Debug.Log("Chartboost: Failed to load Interstitial at:" + location);
        Debug.Log("Chartboost: Error - " + error.ToString());
    }

    void Chartboost_didCacheInterstitial(CBLocation x)
    {
        Debug.Log("Chartboost: Ad loaded!");
    }

}
