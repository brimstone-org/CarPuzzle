using UnityEngine;
using System.Collections;

public class NextLevelButton : MonoBehaviour {

	public void OnClick(){
		int levelCategory = PlayerPrefs.GetInt ("Difficulty");
		int level = PlayerPrefs.GetInt ("Level");

		if (level + 1 <= PlayerPrefs.GetInt ("LevelsIn" + levelCategory)) {
			PlayerPrefs.SetInt ("Level", level + 1);
			UnityEngine.SceneManagement.SceneManager.LoadScene ("LevelScene");
		} else {
			UnityEngine.SceneManagement.SceneManager.LoadScene ("LevelSelect");
		}

	}
}
