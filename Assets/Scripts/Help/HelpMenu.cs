using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HelpMenu : MonoBehaviour {

	public float moveTime = 0.3f;

	[SerializeField]
	Vector3[] tiles;

	[SerializeField]
	GamePlayStartButton button;

	[SerializeField]
	Transform hand;

	[SerializeField]
	Vector3 handOffset;

	[SerializeField]
	GameObject canvasHand;

	void Awake(){
		PlayerPrefs.SetInt ("Difficulty", 1);
		PlayerPrefs.Save ();
	}

	void Start(){

		PlayerPrefs.SetInt ("HadHelp", 1);

		StartCoroutine (Animate ());
	}


	IEnumerator Animate(){

		yield return new WaitForSeconds (moveTime*3);

		WaypointDrawer wp = WaypointDrawer.instance;
		wp.SelectCar (tiles [0]);
		hand.position = tiles [0] + handOffset;
		yield return null;
		for (int i = 0; i <= 15; i++) {
			Vector3 sPos = hand.position;
			float currentTime = 0;
			while (currentTime <= moveTime) {
				hand.position = Vector3.Lerp (sPos, tiles [i] + handOffset, currentTime / moveTime);
				currentTime += Time.deltaTime;
				yield return null;
			}
			wp.UpdateTileList (tiles [i]);
			//yield return new WaitForSeconds (moveTime);
		}
		wp.SelectCar (tiles [16]);
		yield return null;
		for (int i = 17; i <= 31; i++) {
			Vector3 sPos = hand.position;
			float currentTime = 0;
			while (currentTime <= moveTime) {
				currentTime += Time.deltaTime;
				hand.position = Vector3.Lerp (sPos, tiles [i] + handOffset, currentTime / moveTime);
				yield return null;
			}
			wp.UpdateTileList (tiles [i]);
		}
		hand.gameObject.SetActive (false);


		canvasHand.SetActive (true);
		button.OnClick ();
		GameLogic.instance.StartPlaying ();

	}

    void LoadScene()
    {
        StartCoroutine(LoadSceneDelayed());
    }

    IEnumerator LoadSceneDelayed()
    {
        yield return new WaitForSeconds(5);

        if (PlayerPrefs.GetInt("FromMenu") == 1)
            SceneManager.LoadScene("LevelDifficulty");
        else
            SceneManager.LoadScene("MainMenu");
    }

    void OnEnable(){
		GameLogic.instance.winGameEvent += LoadScene;
	}

	void OnDisable(){
		GameLogic.instance.winGameEvent -= LoadScene;
	}
}
