using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour {

	private LevelSerializer.Level level;

	public static LevelBuilder instance;

	public List<GameObject> tiles;
	public List<GameObject> cars;
	public List<GameObject> objectives;

	public string levelName = "Level1_1";

	public string levelPath = "";

	public bool usedInEditor = false;

	public bool debug = false;
	public bool debugLocation = false;

	void Awake () {
		instance = this;

		if (!debug)
			levelName = "Level" + PlayerPrefs.GetInt ("Difficulty") + "_" + PlayerPrefs.GetInt ("Level");

		if (usedInEditor)
			levelName = LevelSerializer.instance.levelName;

	}

	void Start(){
		if (!usedInEditor)
			Build ();
	}

	public int GetId(string tileName){
		for (int i = 0; i < tiles.Count; i++) {
			if (tileName.StartsWith (tiles [i].transform.name))
				return i;
		}
		Debug.Log (tileName);
		return -1;
	}

	public void Build(){
		string json = "";
		if (debugLocation) {
			if (levelPath == "") {
				json = System.IO.File.ReadAllText (Application.persistentDataPath + "/" + levelName + ".json");
			} else {
				json = System.IO.File.ReadAllText (levelPath + "/" + levelName + ".json");

			}
		}
		if(!debugLocation)
			json = ((TextAsset)Resources.Load<TextAsset> ("Levels/" + PlayerPrefs.GetInt("Difficulty") + "/" + levelName)).text;

		level = JsonUtility.FromJson<LevelSerializer.Level> (json);

		for (int i = 0; i < level.tiles.Count; i++) {
			LevelSerializer.TileInfo tile = level.tiles [i];
			GameObject obj = (GameObject)Instantiate (tiles [tile.tileId], new Vector3 (tile.x, -tile.y, 0), Quaternion.Euler (new Vector3 (0, 0, tile.rotation)));
			if(obj.transform.name.StartsWith("RoadLightTile")){
				if (!usedInEditor) {
					RoadLightTile script = obj.GetComponent<RoadLightTile> ();
					script.RedTime = tile.redTime;
					script.GreenTime = tile.greenTime;
					script.Green = tile.green;
					script.startingStepTime = tile.startingStepTime;
					script.StartingColorSettings ();
				}
			}
		}

		if(!usedInEditor)
			GameLogic.instance.cars = new CarScript[6];
		for (int i = 0; i < level.cars.Count; i++) {
			LevelSerializer.CarInfo car = level.cars [i];
			GameObject obj = (GameObject)Instantiate (cars [car.carId], new Vector3 (car.startingX, -car.startingY, 0), Quaternion.Euler (new Vector3 (0, 0, car.startingRotation)));
			if (!usedInEditor) {
				GameLogic.instance.cars [car.carId] = obj.GetComponent<CarScript> ();
				GameLogic.instance.cars [car.carId].startingTime = car.startingTime;
			}
			obj.SetActive (true);
		}

		for (int i = 0; i < level.objectives.Count; i++) {
			LevelSerializer.ObjectiveInfo obj = level.objectives [i];
			GameObject o = (GameObject)Instantiate (objectives[obj.objectiveId], new Vector3 (obj.x, -obj.y, -16), Quaternion.Euler (0, 0, obj.rotation));
			o.SetActive (true);
			if(!usedInEditor)
				o.GetComponent<Objective> ().isDestination = obj.isDestination;
		}
		if(!usedInEditor)
			WaypointDrawer.instance.SelectionPhase = true;

	}

}
