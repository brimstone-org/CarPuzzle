using UnityEngine;
using System.Collections;

public class CirclePulse : MonoBehaviour {

	[SerializeField]
	CarScript car;

	[SerializeField]
	GameObject circle;

	Vector3 circleScale;

	[SerializeField]
	float pulseDuration;
	float currentTime;
	[SerializeField]
	float circlePulseScale;

	bool increasing = true;

	int targetCar;

	// Use this for initialization
	void Start () {
		for(int i=0; i<GameLogic.instance.cars.Length; i++)
			if(car == GameLogic.instance.cars[i])
				targetCar = i;
		circleScale = circle.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {

		if (WaypointDrawer.instance.SelectedCar != targetCar || GameLogic.instance.playing) {
			circle.transform.localScale = circleScale;
			return;
		}

		if(increasing)
			circle.transform.localScale = Vector3.Lerp (circleScale, circlePulseScale * circleScale, currentTime / pulseDuration);
		else
			circle.transform.localScale = Vector3.Lerp (circlePulseScale * circleScale, circleScale, currentTime / pulseDuration);

		currentTime += Time.deltaTime;

		if (currentTime >= pulseDuration) {
			increasing = !increasing;
			currentTime = 0;
		}
	}
}
