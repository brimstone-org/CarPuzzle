using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolveMenu : MonoBehaviour {

	public float moveTime = 0.3f;

	[SerializeField]
	GamePlayStartButton button;

	[SerializeField]
	Transform hand;

	[SerializeField]
	Vector3 handOffset;

	[SerializeField]
	GameObject canvasHand;

	SolutionSolver.Solution solution;

	public bool debug = true;

	void Start(){
		StartCoroutine (Animate ());

		string solutionName = "Solution" + PlayerPrefs.GetInt ("Difficulty") + "_" + PlayerPrefs.GetInt ("Level");	

		string json = "";

		if (debug) {
			json = System.IO.File.ReadAllText (Application.persistentDataPath + "/" + solutionName + ".json");
		}
		else {
			json = ((TextAsset)Resources.Load<TextAsset> ("Levels/" + PlayerPrefs.GetInt("Difficulty") + "s/" + solutionName)).text;
		}

		solution = JsonUtility.FromJson<SolutionSolver.Solution> (json); 
	}


	IEnumerator Animate(){

		yield return new WaitForSeconds (moveTime*3);

		/*WaypointDrawer wp = WaypointDrawer.instance;
		wp.SelectCar (solution.l0 [0]);
		hand.position = solution.l0 [0] + handOffset;
		yield return null;
		for (int i = 0; i <= solution.l0.Count; i++) {
			Vector3 sPos = hand.position;
			float currentTime = 0;
			while (currentTime <= moveTime) {
				hand.position = Vector3.Lerp (sPos, solution.l0 [i] + handOffset, currentTime / moveTime);
				currentTime += Time.deltaTime;
				yield return null;
			}
			wp.UpdateTileList (tiles [i]);
			//yield return new WaitForSeconds (moveTime);
		}*/

		if (solution.l0.Count > 0)
			yield return MoveRoutine (solution.l0);
		if (solution.l1.Count > 0)
			yield return MoveRoutine (solution.l1);
		if (solution.l2.Count > 0)
			yield return MoveRoutine (solution.l2);
		if (solution.l3.Count > 0)
			yield return MoveRoutine (solution.l3);
		if (solution.l4.Count > 0)
			yield return MoveRoutine (solution.l4);
		if (solution.l5.Count > 0)
			yield return MoveRoutine (solution.l5);
			
		hand.gameObject.SetActive (false);

		canvasHand.SetActive (true);
		button.OnClick ();
		GameLogic.instance.StartPlaying ();

	}

	IEnumerator MoveRoutine(List<SolutionSolver.CheckPointInfo> list){
		WaypointDrawer wp = WaypointDrawer.instance;
		wp.SelectCar(FromCheckPointToVector(list[0]));
		hand.position = FromCheckPointToVector(list[0])+ handOffset;
		yield return null;
		for (int i = 0; i < list.Count; i++) {
			Vector3 sPos = hand.position;
			float currentTime = 0;
			while (currentTime <= moveTime) {
				hand.position = Vector3.Lerp (sPos, FromCheckPointToVector(list[i]) + handOffset, currentTime / moveTime);
				currentTime += Time.deltaTime;
				wp.UpdateTileList (FromCheckPointToVector(list[i]));
				yield return null;
			}

		}
	}

	Vector3 FromCheckPointToVector(SolutionSolver.CheckPointInfo info){
		return new Vector3 (info.x, info.y, 0);
	}

	void LoadScene(){
		
	}

	void OnEnable(){
		GameLogic.instance.winGameEvent += LoadScene;
	}

	void OnDisable(){
		GameLogic.instance.winGameEvent -= LoadScene;
	}
}
