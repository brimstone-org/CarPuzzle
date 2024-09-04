using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetLevelNumber : MonoBehaviour {

	[SerializeField]
	Text text;

	// Use this for initialization
	void Awake () {
		text.text = (PlayerPrefs.GetInt ("Level")).ToString();
	}
}
