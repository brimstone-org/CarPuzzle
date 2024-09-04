using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using System;

public class AdMobInterstitials : IAdService
{

    private InterstitialAd currentInterstitial;

    public InterstitialAd CurrentInterstitial
    {
        get {
            return currentInterstitial;
        }

        private set {
            currentInterstitial = value;
        }
    }

    [SerializeField]
    private string appId;
    [SerializeField]
    private string androidId = "ca-app-pub-5151156112378356/8145065025";
    [SerializeField]
    private string iosId = "ca-app-pub-8530091499387924/2522403293";

    string personalizedAds = "0";

    private bool personalized;
    public override bool Personalized
    {
        get { return personalized; }
        set { personalized = value; personalizedAds = personalized ? "1" : "0"; }
    }

    public override void Initialize(string[] args)
    {
        Initialize();
    }

    public override void Initialize()
    {
        MobileAds.Initialize(appId);

        //RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
        //    .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True)
        //    .Build();
        //MobileAds.SetRequestConfiguration(requestConfiguration);
        //AdRequest request = new AdRequest.Builder().TagForChildDirectedTreatment(true).Build();
        //RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
        //    .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True)
        //    .Build();
        //MobileAds.SetRequestConfiguration(requestConfiguration);

        //RequestConfiguration requestConfiguration = MobileAds.getRequestConfiguration()
        //    .toBuilder()
        //    .setTagForChildDirectedTreatment(TAG_FOR_CHILD_DIRECTED_TREATMENT_TRUE)
        //    .build();
        //CurrentInterstitial = RequestInterstitial();
    }

    private InterstitialAd RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = androidId;
#elif UNITY_IPHONE
		string adUnitId = iosId;
#else
                string adUnitId = "unexpected_platform";
#endif

        Debug.Log("ADMOBDebug: Interstitial Request");
        // Initialize an InterstitialAd.
        InterstitialAd interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.


        AdRequest request = new AdRequest.Builder()
         .AddTestDevice(AdRequest.TestDeviceSimulator)
         .AddTestDevice("CB5A1YZ5HM")
         .AddTestDevice("QLF7N15928003477")
         .AddTestDevice("0ad2aa94063d90f9")
         .AddExtra("npa", personalizedAds) //GDPR
         .TagForChildDirectedTreatment(true)
         .AddExtra("max_ad_content_rating", "G")
         .Build();
       
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
        interstitial.OnAdClosed += HandleInterstitialClosed;
        interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
        interstitial.OnAdLoaded += HandleInterstitialAdLoaded;

        return interstitial;
    }

    private void HandleInterstitialAdLoaded(object sender, EventArgs e)
    {
        Debug.Log("ADMOB Interstitial Ad has loaded");
    }

    private void HandleInterstitialFailedToLoad(object sender, EventArgs e)
    {
        Debug.Log("ADMOBDebug: InterstitialFailed to load event received");
    }

    private void HandleInterstitialClosed(object sender, EventArgs e)
    {
        Debug.Log("ADMOBDebug: HandleInterstitialClosed event received");

        InterstitialAd thisAd = (InterstitialAd)sender;

        thisAd.Destroy();
        Debug.Log("ADMOBDebug: HandleInterstitial Destroyed");

        //CurrentInterstitial = RequestInterstitial();
        //DDebug.Log("ADMOBDebug: HandleInterstitial Requested new interstitial");
    }

    public override bool ShowAds()
    {
        if (IsAvailable())
        {
            Debug.Log("ADMOBDebug: Interstitial is loaded and displaying");
            CurrentInterstitial.Show();
            return true;
        }

        Debug.Log("ADMOBDebug: Interstitial not available");
        return false;
    }

    public bool IsAvailable()
    {
        if ((CurrentInterstitial != null) && (CurrentInterstitial.IsLoaded()))
            return true;
        return false;
    }

    public override void Cache()
    {
        if (!IsAvailable())
            CurrentInterstitial = RequestInterstitial();

        //Debug.LogWarning("CACHED ADMOB");
    }
}

