using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class GPGSManager : MonoBehaviour {

	public static GPGSManager instance { get; private set; }

	/*public static string easyAchievement = "CgkIqZmnupgYEAIQAQ";
	public static string mediumAchievement = "CgkIqZmnupgYEAIQAg";
	public static string hardAchievement = "CgkIqZmnupgYEAIQAw";
	public static string hard2Achievement = "CgkIqZmnupgYEAIQBA";*/

	public string[] achievementStrings;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	void Start(){
		#if UNITY_ANDROID
		PlayGamesPlatform.Activate();
		#elif UNITY_IOS
		GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
		#endif
		Social.localUser.Authenticate((bool success) => {
			// handle success or failure
		});
	}
	
	void UnlockAchievement(string achievement){
		Social.ReportProgress(achievement, 100.0f, (bool success) => {
			// handle success or failure
		});
	}

	public void CheckPackCompletedAchievement(int difficulty, int count){

		string achievement = "";

		/*switch (difficulty) {
		case (int)LevelList.Difficulty.easy:
			achievement = easyAchievement;
			break;
		case (int)LevelList.Difficulty.medium:
			achievement = mediumAchievement;
			break;
		case (int)LevelList.Difficulty.hard1:
			achievement = hardAchievement;
			break;
		case (int)LevelList.Difficulty.hard2:
			achievement = hard2Achievement;
			break;
		}*/

		achievement = achievementStrings [difficulty];

		bool ok = true;
		for(int i=1; i<= count; i++){
			if (PlayerPrefs.GetInt (difficulty + "Completed" + i) == 0)
				ok = false;
		}

		if (ok)
			UnlockAchievement (achievement);

	}

}
