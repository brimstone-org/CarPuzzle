using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdMobRewards : RewardsProvider
{
    private RewardBasedVideoAd rewardBasedVideo;

    [SerializeField]
    string androidId;
    [SerializeField]
    string iosId;

    private bool autoCache = false;

    string personalizedAds = "0";

    private bool personalized;
    public override bool Personalized
    {
        get { return personalized; }
        set { personalized = value; personalizedAds = personalized ? "1" : "0"; }
    }

    public void Start()
    {
        // Get singleton reward based video ad reference.
        rewardBasedVideo = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;


#if UNITY_ANDROID
            var info = Placements[0];
            info.placementId = androidId;
            Placements[0] = info;

            RequestRewardedVideo(info.placementId);
#endif

#if UNITY_IOS
            var info = Placements[0];
            info.placementId = iosId;
            Placements[0] = info;

            RequestRewardedVideo(info.placementId);
#endif

    }

    public override void RegisterCallbacks(Action<string> started, Action<string> completed, Action<string> interrupted, Action<String> adLoaded)
    {
        AdStarted = started;
        AdCompleted = completed;
        AdInterrupted = interrupted;
        AdLoaded = adLoaded;
    }

    private void RequestRewardedVideo(string adUnitId)
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder()
         .AddTestDevice(AdRequest.TestDeviceSimulator)
         .AddTestDevice("CB5A1YZ5HM")
         .AddTestDevice("QLF7N15928003477")
         .AddTestDevice("0ad2aa94063d90f9")
         .AddExtra("npa", personalizedAds) //GDPR
         .Build();

        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, adUnitId);
    }


    public override void Cache()
    {
        foreach (var p in Placements)
        {
            Cache(p.localPlacement);
        }
    }

    public override void Cache(string placement)
    {
        Debug.Log("AdMobRewards caching " + placement);
        RequestRewardedVideo(FindPlacementId(placement));
    }

    public override bool IsAvailable()
    {
        return rewardBasedVideo.IsLoaded();
    }

    public override bool IsAvailable(string placement)
    {
        return rewardBasedVideo.IsLoaded();
    }

    public override void PlayAd()
    {
        rewardBasedVideo.Show();
    }

    public override void PlayAd(string placement)
    {
        rewardBasedVideo.Show();
    }

    public override void SetAutoCache(bool auto)
    {
        Debug.Log("AdMobRewards autoCache " + auto);
        autoCache = auto;
    }

#region EventHandlers
    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
        AdLoaded(Placements[0].localPlacement);
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
        AdStarted(Placements[0].localPlacement);
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        AdInterrupted(Placements[0].localPlacement);
        if (autoCache)
            Cache();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardBasedVideoRewarded event received for "
                        + amount.ToString() + " " + type);
        AdCompleted(Placements[0].localPlacement);
        if (autoCache)
            Cache();
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }
    #endregion
}
