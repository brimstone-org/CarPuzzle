using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EditorTile : MonoBehaviour {

	protected LevelSerializer.TileInfo tile = new LevelSerializer.TileInfo ();

	void Start(){
		if(LevelSerializer.instance != null)
			LevelSerializer.instance.onWrite += PrepareLevelData;
	}

    void OnDisable() {
        if (LevelSerializer.instance != null)
            LevelSerializer.instance.onWrite -= PrepareLevelData;
    }

    public virtual void PrepareLevelData(){
		Debug.Log ("Preparing Level Data");

		int x = (int)transform.position.x;
		int y = (int)-transform.position.y;

		tile.tileId = LevelBuilder.instance.GetId (transform.name);
		tile.rotation = (int)transform.localEulerAngles.z;
		tile.x = x;
		tile.y = y;
		LevelSerializer.instance.level.tiles.Add (tile);
	}

	void Update(){
		Vector3 position = transform.position;
		position.x = Mathf.RoundToInt (position.x);
		position.y = Mathf.RoundToInt (position.y);
		transform.position = position;
	}
		
}
