using UnityEngine;
using System.Collections;

public class GridGenerator : MonoBehaviour {

	public float tileSize = 2.56f;
	public int rowCount = 11;
	public int columnCount = 14;

	public Transform gridHolder;

	public float waitTime = 0.3f;

	public GameObject tilePrefab;

	public float scale{ get; private set; }

	public GameLogic logicScript;

	// Use this for initialization
	void Awake () {

		//StartCoroutine (GenerateGrid ());
	}

    /*
	IEnumerator GenerateGrid(){

		logicScript.tiles = new BaseTile[rowCount,columnCount];

		float time = 0;

		for (int i = 0; i < rowCount; i++) {
			for (int j = 0; j < columnCount; j++) {
				GameObject obj = (GameObject)Instantiate (tilePrefab);
				obj.transform.SetParent (gridHolder, false);
				BaseTile tileScript = obj.GetComponent<BaseTile> ();
				tileScript.SetMatrixPosition (i, j);
				obj.transform.position = new Vector3 (j * tileSize, i * -tileSize, 0);
				obj.name = i + "x" + j;
                    logicScript.tiles[i, j] = tileScript;
                    

				if (time >= waitTime) {
					yield return null;
					time = 0;
				}
				time += Time.deltaTime;
			}
		}

		GameLogic.instance.StartPlaying ();
			
	}*/
		
}
