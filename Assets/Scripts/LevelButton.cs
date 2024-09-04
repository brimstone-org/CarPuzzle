using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

	[SerializeField]
	Image image;
	[SerializeField]
	Sprite locked;
	[SerializeField]
	Sprite unlocked;
	[SerializeField]
	Button button;
	[SerializeField]
	Text text;
	int level;

    const bool debugging = false;

	public int Level{
		get{ return level; }
		set{
			level = value;

			string levelCategory = PlayerPrefs.GetInt ("Difficulty").ToString();
            //int lastCompleted = PlayerPrefs.GetInt (levelCategory + "Completed");

			if (PlayerPrefs.GetInt (levelCategory + "Skipped" + (level - 1)) == 1 || PlayerPrefs.GetInt (levelCategory + "Completed" + (level - 1)) == 1 || level == 1 || debugging){
				image.sprite = unlocked;
				button.interactable = true;
				text.text = level.ToString();
			}
			else {
				image.sprite = locked;
				button.interactable = false;
				text.text = "";
			}

			if (PlayerPrefs.GetInt (levelCategory + "Skipped" + level) == 1)
				text.color = Color.red;
		}
	}

	public void OnClick(){
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetString(AsyncLoader.LoadingScenePref, "LevelScene");
        UnityEngine.SceneManagement.SceneManager.LoadScene(AsyncLoader.LoadingSceneName);
    }

}
