using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePlayStartButton : MonoBehaviour {

	[SerializeField]
	Button button;

    [SerializeField]
    Button fastForward;

	bool started = false;

	public static GamePlayStartButton instance { get; private set; }

	void Awake(){
		button.interactable = false;
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (started)
			return;

		bool canInteract = true;
		for (int i = 0; i < GameLogic.instance.cars.Length; i++){
			if (GameLogic.instance.cars [i] == null || GameLogic.instance.objectives[i] == null)
				continue;
			CarScript car = GameLogic.instance.cars [i];
			Vector3 v1 = car.checkPoints [car.checkPoints.Count - 1].transform.position;
			v1.z = 0;
			Vector3 v2 = GameLogic.instance.objectives [i].transform.position;
			v2.z = 0;
			if (Vector3.Distance(v1, v2) >= 1f)
				canInteract = false;
		}
			
		button.interactable = canInteract;
	}

	public void OnClick(){
		started = true;
		button.interactable = false;
        if(fastForward != null)
        fastForward.gameObject.SetActive(true);
	}

	void Reset(){
        fastForward.gameObject.SetActive(false);
		started = false;
	}

	void OnEnable(){
		GameLogic.instance.resetGameEvent += Reset;
	}

	void OnDestroy(){
		GameLogic.instance.resetGameEvent -= Reset;
	}
}
