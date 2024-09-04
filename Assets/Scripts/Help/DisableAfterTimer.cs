using UnityEngine;
using System.Collections;

public class DisableAfterTimer : MonoBehaviour {

	[SerializeField]
	float timer;

	float currentTime;

	void Start(){
		currentTime = 0;
	}

	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;

		if (currentTime > timer)
			gameObject.SetActive (false);
	}
}
