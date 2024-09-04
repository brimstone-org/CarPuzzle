using UnityEngine;
using System.Collections;

public class LightText : MonoBehaviour {

	[SerializeField]
	TextMesh text;
	[SerializeField]
	RoadLightTile tile;

	[SerializeField]
	bool green;

	// Use this for initialization
	void Start () {
		transform.eulerAngles = Vector3.zero;
		if (green)
			text.text = tile.GreenTime.ToString();
		else
			text.text = tile.RedTime.ToString();
	}

	void OnEnable(){
		GameLogic.instance.startGameEvent += StartEvent;
		GameLogic.instance.resetGameEvent += Reset;
	}

	void OnDisable(){
		GameLogic.instance.startGameEvent -= StartEvent;
		//GameLogic.resetGameEvent -= Reset;
	}

	void OnDestroy(){
		GameLogic.instance.resetGameEvent -= Reset;
	}

	void StartEvent(){
		gameObject.SetActive (false);
		//mrenderer.enabled = false;
	}

	void Reset(){
		gameObject.SetActive (true);
		//mrenderer.enabled = true;
	}
}
