using UnityEngine;
using System.Collections;
using NativeInApps;
using System;
using GDPR;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    [SerializeField]
    IAdService[] services;

    public static AdsManager Instance { get; private set; }

    [SerializeField]
    private int showRate = 2;
    public int ShowRate { get { return showRate; } set { showRate = value; } }

    public bool AdsDisabled { get; private set; }

    private bool showPrimary = true;

    private const string disabledAdsPrefString = "No Ads";

    public static string DisabledAdsPrefString
    {
        get
        {
            return disabledAdsPrefString;
        }
    }

    public int LevelsCount { get; private set; }

    public bool ShowedExitAd{ get; set; }

    public event System.Action onExitAdClosed;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
    }

    private IEnumerator Start()
    {
        for (int i = 0; i < services.Length; i++)
        {
            services[i].Personalized = GDPRConsentPanel.Instance.AsBool;
            ////*** Digital content labels ***// https://support.google.com/admob/answer/7562142
            if (i==0)
            {
                RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
                    .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True)
                    .build();
                MobileAds.SetRequestConfiguration(requestConfiguration);

            }

            services[i].Initialize();
        }
      
        RefreshAdsStatus();
        yield return new WaitForSeconds(1);
        PriorityOrder();
    }

    public void UpdatePersonalized()
    {
        foreach (var s in services)
        {
            s.Personalized = GDPRConsentPanel.Instance.AsBool;
        }
    }

    public void PriorityOrder()
    {
        Array.Sort<IAdService>(services);
    }

    public bool ShowAds()
    {

        if (AdsDisabled)
        {
            Debug.Log("Ads disabled");
            return false;
        }

        Debug.Log("Showing Ads");
        PriorityOrder();

        foreach(var s in services)
        {
            if(s.ShowAds())
                return true;
        }

        return false;
    }

    public void UpdateAds()
    {
        //Debug.LogWarning("UPDATE ADS CALLED");
        LevelsCount++;
        TryCache();

        Debug.Log("Update Ads " + LevelsCount);
        if (LevelsCount % showRate == 0)
        {
            if (ShowAds())
            {
                int adsShown = PlayerPrefs.GetInt("AdsShownStats", 0);
                adsShown++;
              //  OneSignalManager.instance.SendTag("AdsShown", adsShown.ToString());
                PlayerPrefs.SetInt("AdsShownStats", adsShown);
                PlayerPrefs.Save();

            }
        }
    }

    public void TryCache()
    {
        //Debug.LogWarning("AD CACHE CALL");
        if ((LevelsCount + 1) % showRate == 0)
        {
            foreach (var s in services)
                s.Cache();
        }
    }

    public static void DisableAds()
    {
        AdsManager.Instance.AdsDisabled = true;
        PlayerPrefs.SetInt(DisabledAdsPrefString, 1);
        PlayerPrefs.Save();
    }

    void OnEnable()
    {
        NativeInApp.OnRefreshCompleted += RefreshAdsStatus;
        NativeInApp.OnItemPurchased += OnItemPurchased;
        GDPRConsentPanel.Instance.OnConsentChange += UpdatePersonalized;
    }

    void OnDisable()
    {
        NativeInApp.OnRefreshCompleted -= RefreshAdsStatus;
        NativeInApp.OnItemPurchased -= OnItemPurchased;
        GDPRConsentPanel.Instance.OnConsentChange -= UpdatePersonalized;
    }

    void OnItemPurchased(string sku)
    {
        if (sku == NativeInApp_IDS.NO_ADS_PRODUCT_ID)
            DisableAds();

        RefreshAdsStatus();
    }

    void RefreshAdsStatus()
    {
        int enabled = PlayerPrefs.GetInt(DisabledAdsPrefString);

        AdsDisabled = enabled == 0 ? false : true;

        Debug.Log(AdsDisabled);
    }

}