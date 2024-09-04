using UnityEngine;
using System.Collections;

public class SkipOption : MonoBehaviour {

	public void Start(){
		string lastSkipped = PlayerPrefs.GetString ("LastSkipDate");
		string date = System.DateTime.Today.ToString();
		if (lastSkipped == date) {
			gameObject.SetActive (false);
		}
	}

	public void OnClick(){
		int levelCategory = PlayerPrefs.GetInt ("Difficulty");
		int level = PlayerPrefs.GetInt ("Level");
		PlayerPrefs.SetString ("LastSkipDate", System.DateTime.Today.ToString());
		if (PlayerPrefs.GetInt (levelCategory + "Completed" + level) == 0)
			PlayerPrefs.SetInt (levelCategory + "Skipped" + level, 1);
		PlayerPrefs.Save ();

		//if(OneSignalManager.instance != null){
		//	string today = System.DateTime.Today.ToString("yyyyMMdd");
		//	OneSignalManager.instance.SendTag ("skipDate", today);
		//}

		UnityEngine.SceneManagement.SceneManager.LoadScene ("LevelSelect");
	}
}
