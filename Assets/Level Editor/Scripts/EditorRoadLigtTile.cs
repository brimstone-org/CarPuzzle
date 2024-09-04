using UnityEngine;
using System.Collections;

public class EditorRoadLigtTile : EditorTile {

	public int greenTime = 2;
	public int redTime = 2;
	public int startingStepTime = 0;
	public bool green;
	
	public override void PrepareLevelData(){
		tile.startingStepTime = startingStepTime;
		tile.redTime = redTime;
		tile.greenTime = greenTime;
		tile.green = green;
		base.PrepareLevelData ();
	}
}
