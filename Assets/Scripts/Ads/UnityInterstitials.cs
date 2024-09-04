using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityInterstitials : IAdService {

    [SerializeField]
    string interstitialPlacemet;

    private bool personalized;
    public override bool Personalized
    {
        get { return personalized; }
        set { personalized = value; }
    }

    public override void Cache()
    {
        Debug.Log("Not supported here");
    }

    public override void Initialize()
    {
        MetaData gdprMetaData = new MetaData("gdpr");
        gdprMetaData.Set("consent", Personalized.ToString().ToLower());
        Advertisement.SetMetaData(gdprMetaData);
        //Debug.Log("Not supported here");
    }

    public override void Initialize(string[] args)
    {
        Debug.Log("Not supported here");
    }

    public override bool ShowAds()
    {
        if (Advertisement.IsReady(interstitialPlacemet))
        {
            Advertisement.Show(interstitialPlacemet);
            return true;
        }
        return false;
    }

}
