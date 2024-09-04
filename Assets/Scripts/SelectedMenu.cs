using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectedMenu : MonoBehaviour {

	public static SelectedMenu instance{ get; private set; }

	public Image car;

	public Text speed;

	public Text startingTime;

	public RectTransform retractButton;
	public Text retractButtonText;

	private bool retracted = false;

	float retractionSize;

	public GameObject[] contents;

	RectTransform rectTransform;

	bool[] contentStartingState;

	void Awake(){
		instance = this;
		if (rectTransform == null)
			rectTransform = GetComponent<RectTransform> ();
	}

	void Start(){
		contentStartingState = new bool[contents.Length];
		for (int i = 0; i < contentStartingState.Length; i++)
			contentStartingState [i] = contents [i].activeInHierarchy;
	}

	public void UpdateValues(Sprite sprite, float spd, float start){
		car.sprite = sprite;
		speed.text = spd.ToString();
		startingTime.text = start.ToString();
	}

	public void Resize(Vector2 size, float minSize, float wantedSize = 0){
		rectTransform.sizeDelta = size;
		retractionSize = rectTransform.sizeDelta.x - wantedSize;
		retractButton.sizeDelta = new Vector2 (wantedSize, retractButton.sizeDelta.y);
		if (wantedSize <= minSize) {
			retractButton.gameObject.SetActive (true);
		} else {
			retractButton.gameObject.SetActive (false);
		}
	}

	public void Retract(){
		Vector3 offset = new Vector3 (retractionSize, 0, 0);
		if (retracted) {
			rectTransform.localPosition = rectTransform.localPosition - offset;
			for (int i = 0; i < contents.Length; i++)
				contents [i].SetActive (contentStartingState[i]);
		} else {
			rectTransform.localPosition = rectTransform.localPosition + offset;
			for (int i = 0; i < contents.Length; i++)
				contents [i].SetActive (false);
		}
		retracted = !retracted;
		if (retracted)
			retractButtonText.text = "<";
		else
			retractButtonText.text = ">";
		
	}

}
