using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using NativeInApps;

public class ButtonOptions : MonoBehaviour {

	[SerializeField]
	Button noAdsButton;

	void Start(){
		if (AdsManager.Instance != null && AdsManager.Instance.AdsDisabled == true)
			noAdsButton.interactable = false;
	}

	public void BuyNoAds(){
	    if (NativeInApp.Instance != null)
	    {
	       // FirebaseAnalyticsWrapper.Instance.DisplayText.text = "Buying no ads";
	        NativeInApp.Instance.BuyProductID(NativeInApp_IDS.NO_ADS_PRODUCT_ID);
        }
           
	}

	public void RefreshPurchases(){
	    if (NativeInApp.Instance != null)
	    {
	        //FirebaseAnalyticsWrapper.Instance.DisplayText.text = "Restoring Purchases";
	        NativeInApp.Instance.RestorePurchases();
        }
           
	}

}
