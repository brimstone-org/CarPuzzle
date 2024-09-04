using UnityEngine;
using System.Collections;

public class ManTravelTile : MonoBehaviour {

	public float distance = 1.4f;

	float time = 0;

	Vector3 position;

	[SerializeField]
	float rotationAngle = 10f;

	Vector3 forward;
	float startingAngle;
	bool going = true;

	// Use this for initialization
	void Awake () {
		position = transform.position;
		forward = transform.up;
		startingAngle = transform.localEulerAngles.z;
		time = 0;
		going = true;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

		if (going) {
			transform.position = Vector3.Lerp (position, position + forward * distance, time / (GameLogic.instance.StepDuration * 2));
			transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, transform.localEulerAngles.y, startingAngle + rotationAngle * Mathf.Sin (time * 10));
		}
		if(time>=GameLogic.instance.StepDuration*2)
			gameObject.SetActive(false);

	}

	public void Jump(){
		going = false;
		transform.position = position + forward * distance;
	}

	void OnEnable(){
		transform.position = position;
		time = 0;
		going = true;
	}
}
