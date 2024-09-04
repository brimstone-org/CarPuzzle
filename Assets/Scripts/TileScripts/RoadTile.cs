using UnityEngine;
using System.Collections;

public class RoadTile : BaseTile {

	// Use this for initialization
	protected override void Start () {
        base.Start();
		passable = true;
	}
	
	// Update is called once per frame
	protected override void Update () {
		

	}

	public override void TimeStep(){
		//Debug.Log ("Road Tile Step");
	}

	public override void TileEffect(CarScript car){
	}

	public override void TileUpdate(CarScript car){
	}
}
