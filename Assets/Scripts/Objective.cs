using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Objective : MonoBehaviour {

	public int targetCar=0;
	public bool isDestination = false;

	public GameObject gradient;
	public ArrowFloat arrow;

	public float arrowPuslseScale = 2f;
	public float pulseDuration = 0.7f;
	float currentTime;
	bool increasing = true;

	Vector3 arrowScale;
	void Start(){
		//GameLogic.instance.cars [targetCar].remainingObjectives++;
		if(isDestination)
			GameLogic.instance.objectives [targetCar] = this;
		if (!isDestination) {
			gradient.SetActive (false);
			arrow.SetStartingPosition (arrow.transform.localPosition + 0.25f * arrow.transform.up);
		}
		arrowScale = arrow.transform.localScale;
	}

	void UpdateObjectives(){
		GameLogic.instance.cars [targetCar].remainingObjectives++;
	}

	void OnEnable(){
		GameLogic.instance.startGameEvent += UpdateObjectives;
	}

	void OnDisable(){
		GameLogic.instance.startGameEvent -= UpdateObjectives;
	}

	void Update(){

		if (!WaypointDrawer.instance.cars [targetCar].gameObject.activeInHierarchy)
			return;

		if (WaypointDrawer.instance.SelectedCar != targetCar || GameLogic.instance.playing) {
			arrow.transform.localScale = arrowScale;
			return;
		}

		if(increasing)
			arrow.transform.localScale = Vector3.Lerp (arrowScale, arrowPuslseScale * arrowScale, currentTime / pulseDuration);
		else
			arrow.transform.localScale = Vector3.Lerp (arrowPuslseScale * arrowScale, arrowScale, currentTime / pulseDuration);

		currentTime += Time.deltaTime;

		if (currentTime >= pulseDuration) {
			increasing = !increasing;
			currentTime = 0;
		}
	}

	void OnCollisionEnter2D(Collision2D other){
        //Debug.Log("CAR ENDED");
        if (other.transform == GameLogic.instance.cars [targetCar].transform && other.collider.transform.tag == "Player") {
            
			if (isDestination && GameLogic.instance.cars [targetCar].remainingObjectives > 1)
				return;

			GameLogic.instance.cars [targetCar].remainingObjectives--;
			if (isDestination && GameLogic.instance.cars [targetCar].remainingObjectives == 0) {
				GameLogic.instance.cars [targetCar].DestinationReached = true;
			}
			//Destroy (gameObject);
			//gameObject.SetActive(false);
		}
	}
}
