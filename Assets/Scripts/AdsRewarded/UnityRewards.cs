using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Profiling.Memory.Experimental;

public class UnityRewards : RewardsProvider
{
    [SerializeField]
    private string gameIdAndroid = "1747660";
    [SerializeField]
    private string gameIdIos = "1747661";

    private string gameId;

    [SerializeField]
    List<string> androidPlacements;

    [SerializeField]
    List<string> iOSPlacements;

    WaitForSeconds checkInterval = new WaitForSeconds(0.1f);

    string[] array;

    ShowOptions options;

    Dictionary<string, bool> status = new Dictionary<string, bool>();

    private bool personalized;
    public override bool Personalized
    {
        get { return personalized; }
        set { personalized = value; }
    }

    private IEnumerator Start()
    {

#if UNITY_ANDROID
        gameId = gameIdAndroid;

        array = new string[androidPlacements.Count];
        androidPlacements.CopyTo(array, 0);

        for (int i = 0; i < Placements.Count; i++)
        {
            var info = Placements[i];
            info.placementId = androidPlacements[i];
            Placements[i] = info;
        }

#else
         gameId = gameIdIos;

        array = new string[iOSPlacements.Count];
        iOSPlacements.CopyTo(array, 0);

        for(int i=0; i<Placements.Count; i++)
        {
            var info = Placements[i];
            info.placementId = iOSPlacements[i];
            Placements[i] = info;
        }
#endif

        if (array.Length == 0)
            yield break;

        foreach (var p in array)
            status[p] = false;

        UnityEngine.Advertisements.MetaData gdprMetaData = new UnityEngine.Advertisements.MetaData("gdpr");
        gdprMetaData.Set("consent", Personalized.ToString().ToLower());
        Advertisement.SetMetaData(gdprMetaData);

        Advertisement.Initialize(gameId);
        options = new ShowOptions();
        options.resultCallback = ProcessResult;

        while (!Advertisement.isInitialized)
            yield return null;

        StartCoroutine(CheckAds());
    }

    void ProcessResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
            AdCompleted(FindPlacementLocal(options.gamerSid));
        else
            AdInterrupted(FindPlacementLocal(options.gamerSid));
    }

    public override void Cache()
    {
        Debug.Log("Not supported here");
    }

    public override void Cache(string placement)
    {
        Debug.Log("Not supported here");
    }

    IEnumerator CheckAds()
    {
        yield return null;

        while (Advertisement.isInitialized)
        {
            foreach (string p in array)
            {
                if (status[p] == false)
                {
                    if (Advertisement.IsReady(p))
                        status[p] = true;

                    AdLoaded(FindPlacementLocal(p));
                }
                else
                {
                    status[p] = Advertisement.IsReady();
                }
            }

            yield return checkInterval;
        }
    }

    public override bool IsAvailable()
    {
        return Advertisement.IsReady();
    }

    public override bool IsAvailable(string placement)
    {
        var id = FindPlacementId(placement);
        return Advertisement.IsReady(id);
    }

    public override void PlayAd()
    {
        Advertisement.Show();
    }

    public override void PlayAd(string placement)
    {
        var id = FindPlacementId(placement);
        options.gamerSid = id;
        status[id] = false;
        AdStarted(placement);
        Advertisement.Show(id, options);

    }

    public override void RegisterCallbacks(Action<string> started, Action<string> completed, Action<string> interrupted, Action<string> adLoaded)
    {
        AdStarted = started;
        AdCompleted = completed;
        AdInterrupted = interrupted;
        AdLoaded = adLoaded;
    }

    public override void SetAutoCache(bool auto)
    {
        Debug.Log("Not supported here");
    }
}
