using UnityEngine;
using System.Collections;

public class ArrowFloat : MonoBehaviour {

    public Vector3 distance;
    public float travelTime = 0.5f;
    private bool movingForward = false;

    private Vector3 startingPosition;
    private float elapsedTime = 0f;

    void Start() {
        //startingPosition = transform.localPosition;
    }

	public void SetStartingPosition(Vector3 newPosition){
		startingPosition = newPosition;
	}

	// Update is called once per frame
	void Update () {

        if (movingForward) {
            transform.localPosition = Vector3.Lerp(startingPosition - distance, startingPosition + distance, elapsedTime / travelTime);
        } else {
            transform.localPosition = Vector3.Lerp(startingPosition + distance, startingPosition - distance, elapsedTime / travelTime);
        }

        if (elapsedTime >= travelTime) {
            elapsedTime = 0;
            movingForward = !movingForward;
        }

        elapsedTime += Time.deltaTime;
	}
}
