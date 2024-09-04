using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {

	public RectTransform selectedMenu;
	public SelectedMenu selectedScript;
    public RectTransform canvas;
    public RectTransform padMenu;
    public float ultraWide = 1.85f;

	public float minMenuSize = 87f;

	[SerializeField]
	float lerpDuration = 1f;
	public float LerpDuration {
		get{ return lerpDuration; }
		private set{ lerpDuration = value; }
	}
	[SerializeField]
	float zoomSize = 2.3f;

	public static CameraScript instance { get; private set; }

	void Awake(){
		instance = this;
	}

	Vector3 start;
	float startSize;

	// Use this for initialization
	void Start () {
		
		Vector3 position = transform.position;
		position.y = -(MapHolder.instance.sizeY - 1) / 2 ;
		//position.x = (MapHolder.instance.sizeX - 1) / 2 - Camera.main.ScreenToWorldPoint(Vector3.zero).x  + 0.5f;
        //if (Camera.main.aspect >= ultraWide)
        position.x = (MapHolder.instance.sizeX - 1) / 2 + 0.5f;

        transform.position = position;


		float distance;
		Vector2 sizeDelta = selectedMenu.sizeDelta;
        distance = WorldToCanvasPosition(canvas, Camera.main, new Vector3(MapHolder.instance.sizeX-0.5f, 0, 0)).x;
        //Debug.Log(distance);
		sizeDelta.x = (canvas.sizeDelta.x/2 - distance) > minMenuSize ? (canvas.sizeDelta.x/2 - distance) : minMenuSize;
		selectedScript.Resize (sizeDelta, minMenuSize, canvas.sizeDelta.x/2 - distance);
        //if (Camera.main.aspect >= ultraWide)
            padMenu.sizeDelta = new Vector2(canvas.sizeDelta.x / 2 - distance, sizeDelta.y);

		start = Camera.main.transform.position;
		startSize = Camera.main.orthographicSize;
	}

    private Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position) {
        //Vector position (percentage from 0 to 1) considering camera size.
        //For example (0,0) is lower left, middle is (0.5,0.5)
        Vector2 temp = camera.WorldToViewportPoint(position);

        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
        temp.x *= canvas.sizeDelta.x;
        temp.y *= canvas.sizeDelta.y;

        //The result is ready, but, this result is correct if canvas recttransform pivot is 0,0 - left lower corner.
        //But in reality its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
        //We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
        //returned value will still be correct.

        temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
        temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

        return temp;
    }
		
	public void Lerp(Vector3 target){

		// x 10.3 y -1.5
		// x 2.7 y -8.5

		if (target.x < 2.7f)
			target.x = 2.7f;
		if (target.x > 10.3f)
			target.x = 10.3f;
		if (target.y > -1.5f)
			target.y = -1.5f;
		if (target.y < -8.5f)
			target.y = -8.5f;

		//StartCoroutine (LerpCamera (target, lerpDuration));
		StartCoroutine(CameraShake(target, lerpDuration));

	}

	IEnumerator LerpCamera(Vector3 target, float duration){
		float currentTime = 0;
		while (currentTime <= duration) {
			Camera.main.transform.position = Vector3.Lerp (start, target, currentTime / duration);
			Camera.main.orthographicSize = Mathf.Lerp (startSize, zoomSize, currentTime / duration);
			currentTime += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator CameraShake(Vector3 target, float duration){

		float val = 1;

		Vector3 startPos = transform.position;

		Vector3 dir = new Vector3 (Mathf.Cos (Random.Range (0, 101) / 10f), Mathf.Sin (Random.Range (0, 101) / 10f), 0);

		float currentTime = 0;

		float startVal = val;
		while (currentTime <= duration) {
		
			val = Mathf.Lerp (startVal, 0, currentTime / duration);

			float sinArg = Mathf.Lerp (0, 2 * Mathf.PI, currentTime / duration);

			transform.position = Vector3.LerpUnclamped(startPos, startPos + dir, Mathf.Sin(sinArg) * val);
			//Debug.Log (Mathf.Sin (sinArg) * val);
			currentTime += Time.deltaTime;
			yield return null;

		}
			
		StartCoroutine (LerpCamera (target, lerpDuration));
	}

	public void Reset(){
		transform.position = start;
		Camera.main.orthographicSize = startSize;
	}

	void OnEnable(){
		GameLogic.instance.resetGameEvent += Reset;
	}

	void OnDisable(){
		GameLogic.instance.resetGameEvent -= Reset;
	}

}
