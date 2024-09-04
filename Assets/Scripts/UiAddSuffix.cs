using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UiAddSuffix : MonoBehaviour {

	public string suffix;

	[SerializeField]
	Text text;

	// Use this for initialization
	void Start () {
		text.text += suffix;
	}
		
}
