using UnityEngine;
using System.Collections;

public class BuildingTile : BaseTile {

	// Use this for initialization
	protected override void Start () {
        base.Start();
		passable = false;
	}

	// Update is called once per frame
	protected override void Update () {


	}

	public override void TimeStep(){
	}

	public override void TileEffect(CarScript car){
	}

	public override void TileUpdate(CarScript car){
	}
}
