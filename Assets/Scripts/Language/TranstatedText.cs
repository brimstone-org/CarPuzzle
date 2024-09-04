using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TranstatedText : MonoBehaviour {

	public string key;

	Text text;

	private bool skipEnable = true;

	public bool toUpper = false;

	void Start() {
		text = GetComponent<Text>();
		UpdateText();
		skipEnable = false;
	}

	void OnEnable(){
		if (skipEnable)
			return;
		//Debug.Log ("Enable");
		UpdateText ();
	}

	public void UpdateText() {
		if (LanguageManager.instance == null)
			return;
		text.font = LanguageManager.instance.GetFont ();
		text.text = LanguageManager.instance.Get(key);

		if (toUpper)
			text.text = text.text.ToUpper ();
	}

}