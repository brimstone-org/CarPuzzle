using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSerializer : MonoBehaviour {

	public string savePath = "";
	public string levelName = "Level1_1";

	public static LevelSerializer instance;

	public delegate void OnWrite();
	public event OnWrite onWrite;

	[System.Serializable]
	public class TileInfo{
		public int tileId;
		public int x;
		public int y;
		public int rotation;
		public int greenTime;
		public int redTime;
		public bool green;
		public int startingStepTime;
	}

	[System.Serializable]
	public class CarInfo{
		public int carId;
		public int startingX;
		public int startingY;
		public int startingTime;
		public int startingRotation;
	}

	[System.Serializable]
	public class ObjectiveInfo{
		public int objectiveId;
		public bool isDestination;
		public int x;
		public int y;
		public int rotation;
	}

	[System.Serializable]
	public class Level{
		public List<TileInfo> tiles = new List<TileInfo> ();
		public List<CarInfo> cars = new List<CarInfo> ();
		public List<ObjectiveInfo> objectives = new List<ObjectiveInfo> ();
	}

	public Level level;

	void Awake(){
		instance = this;
		level = new Level();
	}

	public void Write(){
		onWrite ();
		string json = JsonUtility.ToJson (level, true);
		if (savePath == "") {
			System.IO.File.WriteAllText (Application.persistentDataPath + "/" + levelName + ".json", json);
			Debug.Log ("Level Written at: " + Application.persistentDataPath + "/" + levelName + ".json");
		}
		else {
			System.IO.File.WriteAllText (savePath + "/" + levelName + ".json", json);
			Debug.Log (savePath + "/" + levelName + ".json");
		}

	}
}
