using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdMobInitializer : MonoBehaviour {

    [SerializeField]
    string androidAppId;

    [SerializeField]
    string iosAppid;

	// Use this for initialization
	void Awake () {
#if UNITY_ANDROID
        string appId = androidAppId;
#elif UNITY_IPHONE
            string appId = iosAppid;
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);


    }

}
