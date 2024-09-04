using UnityEngine;
using System.Collections;

public class TitleTranslator : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		if (LanguageManager.instance.language == SystemLanguage.English.ToString())
			Destroy (gameObject);
	}

}
