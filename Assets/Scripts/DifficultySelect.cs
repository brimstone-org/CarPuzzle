using UnityEngine;
using System.Collections;

public class DifficultySelect : MonoBehaviour {

	[SerializeField]
	LevelList.Difficulty difficulty;

	public void OnClick(){
		PlayerPrefs.SetInt ("Difficulty", (int)difficulty);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("LevelSelect");
	}
}
