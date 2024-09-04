using UnityEngine;
using System.Collections;

public class LevelList : MonoBehaviour
{

    public enum Difficulty
    {
        easy1 = 1,
        medium,
        hard1,
        hard2,
        easy2,
        medium2

    }

    /*[SerializeField]
	int easyCount;
	[SerializeField]
	int mediumCount;
	[SerializeField]
	int hardCount;
	[SerializeField]
	int hardCount1;
	[SerializeField]
	int hardCount2;*/

    [SerializeField]
    int[] levelCount;

    [SerializeField]
    GameObject levelButtonPrefab;

    [SerializeField]
    GameObject buttonsHolder;

    Difficulty difficulty;

    void Start()
    {
        difficulty = (Difficulty)PlayerPrefs.GetInt("Difficulty");

        /*PlayerPrefs.SetInt ("LevelsIn" + ((int)Difficulty.easy).ToString(), easyCount);
		PlayerPrefs.SetInt ("LevelsIn" + ((int)Difficulty.medium).ToString(), mediumCount);
		PlayerPrefs.SetInt ("LevelsIn" + ((int)Difficulty.hard).ToString(), hardCount);
		PlayerPrefs.SetInt ("LevelsIn" + ((int)Difficulty.hard1).ToString(), hardCount1);
		PlayerPrefs.SetInt ("LevelsIn" + ((int)Difficulty.hard2).ToString(), hardCount2);*/
        PlayerPrefs.SetInt("LevelsIn" + (int)difficulty, levelCount[(int)difficulty]);

        int count = 0;

        /*switch (difficulty) {
		case Difficulty.easy: 
			count = easyCount;
			break;
		case Difficulty.medium:
			count = mediumCount;
			break;
		case Difficulty.hard:
			count = hardCount;
			break;
		default:
			UnityEngine.SceneManagement.SceneManager.LoadScene ("MainMenu");
			break;
		}*/

        count = levelCount[(int)difficulty];
        for (int i = 1; i <= count; i++)
        {
            GameObject obj = (GameObject)Instantiate(levelButtonPrefab);
            obj.transform.SetParent(buttonsHolder.transform);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<LevelButton>().Level = i;
        }
    }
}
