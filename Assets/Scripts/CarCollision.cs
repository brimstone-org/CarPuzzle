using UnityEngine;
using System.Collections;

public class CarCollision : MonoBehaviour {

    public GameObject boomPrefab;

	[SerializeField]
	private static float delay = 0.2f;

	void OnCollisionEnter2D(Collision2D other){
		//Debug.Log (other.transform);
		if (GameLogic.instance.playing == false)
			return;

		if (other.collider.transform.tag == transform.tag) {
			GameObject obj = (GameObject)Instantiate(boomPrefab, other.contacts[0].point, Quaternion.identity);
			CameraScript.instance.Lerp(new Vector3(other.contacts [0].point.x, other.contacts[0].point.y,0) + Vector3.forward * -100f);
			GameLogic.instance.GameLost (CameraScript.instance.LerpDuration * 2 + delay);
			Destroy (obj, CameraScript.instance.LerpDuration * 2 + delay);
		}
	}


}
