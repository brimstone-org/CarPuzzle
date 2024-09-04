using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolutionSolver : MonoBehaviour {

	[System.Serializable]
	public class CheckPointInfo{
		public int index;
		public int x;
		public int y;
		public CheckPointInfo(int index, int x, int y){
			this.index = index;
			this.x = x;
			this.y = y;
		}
	}

 	string solutionName;

	[System.Serializable]
	public class Solution{
		public List<CheckPointInfo>[] checkpoints = new List<CheckPointInfo>[6];
		public List<CheckPointInfo> l0;
		public List<CheckPointInfo> l1;
		public List<CheckPointInfo> l2;
		public List<CheckPointInfo> l3;
		public List<CheckPointInfo> l4;
		public List<CheckPointInfo> l5;

		public Solution(){
			for(int i=0; i<checkpoints.Length; i++){
				checkpoints[i] = new List<CheckPointInfo>();
			}
			l0 = checkpoints[0];
			l1 = checkpoints[1];
			l2 = checkpoints[2];
			l3 = checkpoints[3];
			l4 = checkpoints[4];
			l5 = checkpoints[5];
		}

	}

	[SerializeField]
	Solution solution;

	// Use this for initialization
	void Start () {
		solution = new Solution ();
		solutionName = "Solution" + PlayerPrefs.GetInt ("Difficulty") + "_" + PlayerPrefs.GetInt ("Level");	
	}
	
	public void Write(){
		for (int i = 0; i < solution.checkpoints.Length; i++) {
			CarScript car = GameLogic.instance.cars [i];
			if (car == null)
				continue;
			for (int j = 0; j < car.checkPoints.Count; j++) {
				solution.checkpoints[i].Add(new CheckPointInfo(j, Mathf.RoundToInt(car.checkPoints[j].transform.position.x) , Mathf.RoundToInt(car.checkPoints[j].transform.position.y)));
			}
		}
			
		string json = JsonUtility.ToJson (solution, true);

		Debug.Log (json);

		System.IO.File.WriteAllText (Application.persistentDataPath + "/" + solutionName + ".json", json);
		Debug.Log ("Level Written at: " + Application.persistentDataPath + "/" + solutionName + ".json");


	}
}
