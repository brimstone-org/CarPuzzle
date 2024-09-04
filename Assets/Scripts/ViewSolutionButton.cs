using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using NativeInApps;
using UnityEngine.UI;

public class ViewSolutionButton : MonoBehaviour {

    string levelName;
    bool unlocked = false;

    string placementId = "viewsolution";

    [SerializeField]
    GameObject panel;

    private void Start()
    {
        levelName = "Level" + PlayerPrefs.GetInt("Difficulty") + "_" + PlayerPrefs.GetInt("Level");
        if (PlayerPrefs.GetInt("unlocked_solution_" + levelName, 0) == 1)
            unlocked = true;
    }

    public void OnClick(){

        //for IAP users
        if (PlayerPrefs.GetInt("Unlock Solutions") == 1)
        {
           // FirebaseAnalyticsWrapper.Instance.DisplayText.text = "Already bought unlock solutions";
            LoadSolution();
            return;
        }

        if (unlocked)
        {
           // FirebaseAnalyticsWrapper.Instance.DisplayText.text = "This solution is unlocked";
            LoadSolution();
            return;
        }

        if(!Rewards.Instance.IsAvailable(placementId))
        {
            //FirebaseAnalyticsWrapper.Instance.DisplayText.text = "Video not available";
            LoadSolution();
            return;
        }


       // FirebaseAnalyticsWrapper.Instance.DisplayText.text = "Outside VideoAds is " + FirebaseAnalyticsWrapper.Instance.VideoAds.ToString();
        panel.gameObject.SetActive(true);

		/*if (NativeInApp.Instance != null) {
			Debug.Log ("Trying to buy");
            NativeInApp.Instance.BuyProductID (NativeInApp_IDS.LEVEL_SOLUTIONS_PRODUCT_ID);
		}*/

	}

    public void WatchVideoForSolution()
    {
        Rewards.OnAdCompleted += Rewards_OnAdCompleted;
        Rewards.Instance.PlayAd(placementId);
    }

    private void Rewards_OnAdCompleted(string placement)
    {
        if (this.placementId != placement)
            return;

        PlayerPrefs.SetInt("unlocked_solution_" + levelName, 1);
        Rewards.OnAdCompleted -= Rewards_OnAdCompleted;
        LoadSolution();
    }


    public void LoadSolution()
    {
        SceneManager.LoadScene("SolveScene");
    }
}
