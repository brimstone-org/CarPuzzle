using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using NativeInApps;

[RequireComponent(typeof(DifficultySelect))]
public class LevelPackUnlocker : MonoBehaviour {

	public string purchaseName = "Pack Heavy 2";

	DifficultySelect script;

	[SerializeField]
	GameObject lockedSprite;

	bool bought = false;

	void Awake(){
		script = GetComponent<DifficultySelect> ();
	}

	void Start () {
		Refresh ();
	}
	
	void Refresh(string sku = ""){
		if (PlayerPrefs.GetInt (purchaseName) != 1) {
			lockedSprite.SetActive (true);
			bought = false;
		} else {
			lockedSprite.SetActive (false);
			bought = true;
		}
	}

	public void OnClick(){
		if (bought) {
			//SceneManager.LoadScene ("LevelSelect");
			script.OnClick();
		}
		else {
			if (NativeInApp.Instance != null)
                NativeInApp.Instance.BuyProductName (purchaseName);
		}
	}

	void OnEnable(){
        NativeInApp.OnItemPurchased += Refresh;
	}

	void OnDisable(){
        NativeInApp.OnItemPurchased -= Refresh;
	}
}
